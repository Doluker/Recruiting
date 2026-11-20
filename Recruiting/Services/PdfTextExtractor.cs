using Recruiting.Interfaces;
using System.Text;
using UglyToad.PdfPig;



namespace Recruiting.Services
{
    public class PdfTextExtractor: IPdfTextExtractor
    {
        public Task<string> ExtractTextAsync(Stream pdfStream, CancellationToken ct = default)
        {
            var sb = new StringBuilder();
            using var document = PdfDocument.Open(pdfStream);
            foreach (var page in document.GetPages())
            {
                ct.ThrowIfCancellationRequested();
                var words = page.GetWords().ToList();
                if (words.Count == 0) continue;
                // Группируем слова по высоте Y (с погрешностью в 3 пункта для одной строки)
                // И сортируем внутри строки по координате X (слева направо)
                var lines = words
                    .GroupBy(w => Math.Round(w.BoundingBox.Bottom / 3.0) * 3.0)
                    .OrderByDescending(g => g.Key) // сверху вниз
                    .Select(g => string.Join(" ", g.OrderBy(w => w.BoundingBox.Left).Select(w => w.Text)));
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        sb.AppendLine(line);
                }
                sb.AppendLine();
            }
            return Task.FromResult(sb.ToString().Trim());
        }
    }
}
