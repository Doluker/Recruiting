using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class StatusHistory
{
    public int Id { get; set; }

    public int ApplicationId { get; set; }

    public int? FromStatusId { get; set; }

    public int ToStatusId { get; set; }

    public string? Comment { get; set; }

    public DateTime ChangedAt { get; set; }

    public virtual Application Application { get; set; } = null!;

    public virtual FunnelStatus? FromStatus { get; set; }

    public virtual FunnelStatus ToStatus { get; set; } = null!;
}
