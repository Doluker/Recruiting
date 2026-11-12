using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    public record ActivityLogDTO(
        long Id,
        int? UserId,
        string? UserName,
        string Action,
        string? Entity,
        int? EntityId,
        string? Details,
        DateTime CreatedAt);

    public record ActivityLogFilter(
        string? Action,
        string? Entity,
        int? UserId,
        DateTime? From,
        DateTime? To,
        int Page = 1,
        int PageSize = 50);

}
