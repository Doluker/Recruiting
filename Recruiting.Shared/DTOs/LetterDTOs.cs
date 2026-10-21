using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    public record LetterDto(
        int Id,
        int ApplicationId,
        int KindId,
        string KindName,
        int StatusId,
        string StatusName,
        string Subject,
        string Body,
        DateTime CreatedAt,
        DateTime? SentAt);

    public record CreateLetterRequest(int ApplicationId, int KindId, string Subject, string Body);

    public record UpdateLetterRequest(string Subject, string Body);

    /// <summary>Поставить письмо в очередь на отправку.</summary>
    public record SendLetterRequest(int LetterId);

    public record LetterFilter(
        int? ApplicationId,
        int? StatusId,
        int? KindId,
        int Page = 1,
        int PageSize = 20);
}
