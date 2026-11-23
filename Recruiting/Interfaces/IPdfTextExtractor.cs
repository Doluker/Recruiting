namespace Recruiting.Interfaces
{
    public interface IPdfTextExtractor
    {
        Task<string> ExtractTextAsync(Stream pdfStream, CancellationToken ct = default);
    }
}
