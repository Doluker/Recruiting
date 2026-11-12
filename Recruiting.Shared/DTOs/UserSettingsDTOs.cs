using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    /// <summary>
    /// Секретный токен клиенту не отдаём — только признак, что он есть.
    /// </summary>
    public record UserSettingsDTO(int Id, bool HasHhToken, DateTime UpdatedAt);

    public record UpdateUserSettingsRequest(string? HhToken);
}
