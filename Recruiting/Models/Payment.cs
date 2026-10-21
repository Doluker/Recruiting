using System;
using System.Collections.Generic;

namespace Recruiting.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PlanId { get; set; }

    public string? ProviderPaymentId { get; set; }

    public decimal Amount { get; set; }

    public int CurrencyId { get; set; }

    public int PaymentStatusId { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual Plan Plan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
