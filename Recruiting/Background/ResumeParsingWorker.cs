using Recruiting.Interfaces;
using Recruiting.Models;
using Recruiting.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Recruiting.Background
{
    public class ResumeParsingWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ResumeParsingWorker> _logger;
        public ResumeParsingWorker(IServiceProvider serviceProvider, ILogger<ResumeParsingWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Фоновый воркер обработки резюме запущен.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    bool processedAny = await ProcessNextJobAsync(stoppingToken);

                    // Если задач не было — ждем 3 секунды перед следующим опросом базы
                    if (!processedAny)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка в фоновом воркере обработки резюме");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
        private async Task<bool> ProcessNextJobAsync(CancellationToken ct)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
            var storage = scope.ServiceProvider.GetRequiredService<IStorageService>();
            var extractor = scope.ServiceProvider.GetRequiredService<IPdfTextExtractor>();
            // 1. Ищем задачу: со статусом Pending или зависшую (в работе > 10 минут)
            var now = DateTime.UtcNow;
            var job = await db.ParseJobs
                .Include(j => j.ResumeFile)
                .Where(j => j.ParseJobStatusId == (int)ParseJobStatusEnum.Pending ||
                           (j.ParseJobStatusId == (int)ParseJobStatusEnum.Running && j.LockedUntil < now))
                .OrderBy(j => j.CreatedAt)
                .FirstOrDefaultAsync(ct);
            if (job == null) return false;
            // 2. Блокируем задачу под себя (защита от повторного взятия)
            job.ParseJobStatusId = (int)ParseJobStatusEnum.Running;
            job.StartedAt = DateTime.UtcNow;
            job.LockedUntil = DateTime.UtcNow.AddMinutes(10);
            job.Attempts++;
            await db.SaveChangesAsync(ct);
            try
            {
                _logger.LogInformation("Обработка резюме #{ResumeFileId} (попытка {Attempt})...", job.ResumeFileId, job.Attempts);
                // 3. Скачиваем PDF из MinIO
                await using var pdfStream = await storage.DownloadAsync(job.ResumeFile.ObjectKey, ct);
                // 4. Извлекаем текст
                var text = await extractor.ExtractTextAsync(pdfStream, ct);
                if (string.IsNullOrWhiteSpace(text))
                {
                    // Если текста нет вообще (возможно, скан) — помечаем для ручной проверки
                    job.ParseJobStatusId = (int)ParseJobStatusEnum.NeedsManualReview;
                    job.Error = "В PDF отсутствует текстовый слой (возможно, скан).";
                }
                else
                {
                    job.ResumeFile.ExtractedText = text;
                    job.ResumeFile.ProcessedAt = DateTime.UtcNow;
                    job.ParseJobStatusId = (int)ParseJobStatusEnum.Completed;
                    job.Error = null;
                }
                job.FinishedAt = DateTime.UtcNow;
                await db.SaveChangesAsync(ct);
                _logger.LogInformation("Резюме #{ResumeFileId} успешно обработано.", job.ResumeFileId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обработки резюме #{ResumeFileId}", job.ResumeFileId);
                job.Error = ex.Message;
                if (job.Attempts >= 3)
                {
                    job.ParseJobStatusId = (int)ParseJobStatusEnum.Failed;
                    job.FinishedAt = DateTime.UtcNow;
                }
                else
                {
                    // Возвращаем в очередь на повторную попытку
                    job.ParseJobStatusId = (int)ParseJobStatusEnum.Pending;
                    job.LockedUntil = null;
                }
                await db.SaveChangesAsync(ct);
            }
            return true;
        }
    }
}
