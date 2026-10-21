using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class ActivityLog
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? Entity { get; set; }

    public int? EntityId { get; set; }

    public string? Details { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? User { get; set; }
}
