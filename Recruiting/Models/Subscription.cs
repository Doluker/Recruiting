using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Subscription
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public DateTime StartedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Plan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
