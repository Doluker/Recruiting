using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class FunnelStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<StatusHistory> StatusHistoryFromStatuses { get; set; } = new List<StatusHistory>();

    public virtual ICollection<StatusHistory> StatusHistoryToStatuses { get; set; } = new List<StatusHistory>();
}
