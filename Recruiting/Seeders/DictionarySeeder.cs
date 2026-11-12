using Microsoft.EntityFrameworkCore;
using Recruiting.Models;
using Recruiting.Shared.Enums;

namespace Recruiting.Seeders;

public static class DictionarySeeder
{
    /// <summary>
    /// Заполняет справочники. Id берутся из enum'ов — поэтому enum и БД не могут разъехаться.
    /// Идемпотентно: недостающие строки добавляются, существующие не трогаются.
    /// </summary>
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();

        // ── Справочники с enum'ами ──────────────────────────────────────
        await Seed(db.Roles, new List<Role>
        {
            new() { Id = (int)RoleEnum.Admin, Name = "Админ" },
            new() { Id = (int)RoleEnum.Hr,     Name = "HR" }
        }, r => r.Id);

        await Seed(db.Sources, new List<Source>
        {
            new() { Id = (int)SourceEnum.Manual,     Name = "Вручную" },
            new() { Id = (int)SourceEnum.Hh,         Name = "hh.ru" },
            new() { Id = (int)SourceEnum.HabrCareer, Name = "Habr Career" },
            new() { Id = (int)SourceEnum.Nejkareru,  Name = "Nejkareru" }
        }, s => s.Id);

        await Seed(db.VacancyStatuses, new List<VacancyStatus>
        {
            new() { Id = (int)VacancyStatusEnum.Draft,    Name = "Черновик" },
            new() { Id = (int)VacancyStatusEnum.Active,   Name = "Активна" },
            new() { Id = (int)VacancyStatusEnum.Closed,   Name = "Закрыта" },
            new() { Id = (int)VacancyStatusEnum.Archived, Name = "В архиве" }
        }, s => s.Id);

        await Seed(db.FunnelStatuses, new List<FunnelStatus>
        {
            new() { Id = (int)FunnelStatusEnum.New,       Name = "Новый" },
            new() { Id = (int)FunnelStatusEnum.Screening, Name = "Скрининг" },
            new() { Id = (int)FunnelStatusEnum.Invited,   Name = "Приглашён" },
            new() { Id = (int)FunnelStatusEnum.Interview, Name = "Интервью" },
            new() { Id = (int)FunnelStatusEnum.Rejected,  Name = "Отказ" },
            new() { Id = (int)FunnelStatusEnum.Hired,     Name = "Нанят" }
        }, s => s.Id);

        await Seed(db.LetterKinds, new List<LetterKind>
        {
            new() { Id = (int)LetterKindEnum.Invitation, Name = "Приглашение" },
            new() { Id = (int)LetterKindEnum.Rejection,  Name = "Отказ" },
            new() { Id = (int)LetterKindEnum.Screening,  Name = "Скрининг" }
        }, s => s.Id);

        await Seed(db.LetterStatuses, new List<LetterStatus>
        {
            new() { Id = (int)LetterStatusEnum.Queued, Name = "В очереди" },
            new() { Id = (int)LetterStatusEnum.Sent,   Name = "Отправлено" },
            new() { Id = (int)LetterStatusEnum.Failed, Name = "Ошибка" }
        }, s => s.Id);

        await Seed(db.ParseJobStatuses, new List<ParseJobStatus>
        {
            new() { Id = (int)ParseJobStatusEnum.Pending,           Name = "Ожидает" },
            new() { Id = (int)ParseJobStatusEnum.Running,           Name = "В работе" },
            new() { Id = (int)ParseJobStatusEnum.Completed,         Name = "Готово" },
            new() { Id = (int)ParseJobStatusEnum.Failed,            Name = "Ошибка" },
            new() { Id = (int)ParseJobStatusEnum.NeedsManualReview, Name = "Требует ручной проверки" }
        }, s => s.Id);

        await Seed(db.PaymentStatuses, new List<PaymentStatus>
        {
            new() { Id = (int)PaymentStatusEnum.Pending,  Name = "Ожидает оплату" },
            new() { Id = (int)PaymentStatusEnum.Paid,     Name = "Оплачено" },
            new() { Id = (int)PaymentStatusEnum.Failed,   Name = "Ошибка оплаты" },
            new() { Id = (int)PaymentStatusEnum.Refunded, Name = "Возврат" }
        }, s => s.Id);

        await Seed(db.Currencies, new List<Currency>
        {
            new() { Id = (int)CurrencyEnum.Rub, Name = "Рубль",  Symbol = "₽" },
            new() { Id = (int)CurrencyEnum.Usd, Name = "Доллар", Symbol = "$" },
            new() { Id = (int)CurrencyEnum.Eur, Name = "Евро",  Symbol = "€" }
        }, c => c.Id);

        await Seed(db.ResumeContentTypes, new List<ResumeContentType>
        {
            new() { Id = (int)ResumeContentTypeEnum.Pdf,  Name = "application/pdf" },
            new() { Id = (int)ResumeContentTypeEnum.Doc,  Name = "application/msword" },
            new() { Id = (int)ResumeContentTypeEnum.Docx, Name = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" }
        }, s => s.Id);

        // ── Тарифы: без enum'а (цены и лимиты меняются) ─────────────────
        await Seed(db.Plans, new List<Plan>
        {
            new()
            {
                Id = 1, Name = "Free", PriceMonthly = null,
                MaxUploadsPerMonth = 3, MaxActiveVacancies = 3,
                CanParse = false, CanExportExcel = false, HasFullDashboard = false
            },
            new()
            {
                Id = 2, Name = "Pro", PriceMonthly = 990m,
                MaxUploadsPerMonth = null, MaxActiveVacancies = null,
                CanParse = true, CanExportExcel = true, HasFullDashboard = true
            }
        }, p => p.Id);

        await db.SaveChangesAsync();

        await FixSequencesAsync(db);
    }

    /// <summary>Добавляет только те строки, которых ещё нет (по Id).</summary>
    private static async Task Seed<T>(DbSet<T> set, List<T> items, Func<T, int> idSelector)
        where T : class
    {
        var existing = await set.ToListAsync();
        var existingIds = existing.Select(idSelector).ToHashSet();
        var missing = items.Where(x => !existingIds.Contains(idSelector(x))).ToList();

        if (missing.Count > 0)
            await set.AddRangeAsync(missing);
    }

    /// <summary>
    /// Сид с id=0 ломает sequence (identity стартует с 1) → следующий INSERT конфликтует по PK.
    /// Сдвигаем sequence за max(id).
    /// </summary>
    private static async Task FixSequencesAsync(DataBaseContext db)
    {
        string[] tables =
        [
            "Roles", "Sources", "Cities", "Educations", "Positions", "Skills",
            "VacancyStatuses", "FunnelStatuses", "LetterKinds", "ParseJobStatuses",
            "PaymentStatuses", "Plans", "Currencies", "ResumeContentTypes"
        ];

        foreach (var table in tables)
        {
            var sql = string.Format("""SELECT setval('"{0}_Id_seq"', COALESCE((SELECT MAX("Id") FROM "{0}"), 0) + 1, false);""", table);
            await db.Database.ExecuteSqlRawAsync(sql);
        }
    }
}