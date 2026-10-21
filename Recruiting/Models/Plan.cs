using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Plan
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal? PriceMonthly { get; set; }

    public int? MaxUploadsPerMonth { get; set; }

    public int? MaxActiveVacancies { get; set; }

    public bool CanParse { get; set; }

    public bool CanExportExcel { get; set; }

    public bool HasFullDashboard { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
