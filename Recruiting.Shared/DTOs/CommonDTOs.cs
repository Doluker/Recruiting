using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    /// <summary>Страница результата любого списка.</summary>
    public record PagedResult<T>(List<T> Items, int Total, int Page, int PageSize)
    {
        public int Pages => PageSize <= 0 ? 0 : (int)Math.Ceiling(Total / (double)PageSize);
    }

    /// <summary>Единый формат ошибки для всех эндпоинтов.</summary>
    public record ApiError(string Code, string Message, Dictionary<string, string[]>? Errors = null);

    /// <summary>Результат операции без тела ответа.</summary>
    public record OperationResult(bool Success, string? Message, int? EntityId = null);
}
