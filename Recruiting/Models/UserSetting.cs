using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class UserSetting
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? HhTokenEncrypted { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
