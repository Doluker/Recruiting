using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    public record PlanDTO(
        int Id,
        string Name,
        decimal? PriceMonthly,
        int? MaxUploadsPerMonth,
        int? MaxActiveVacancies,
        bool CanParse,
        bool CanExportExcel,
        bool HasFullDashboard);

    public record SubscriptionDTO(
        int Id,
        int PlanId,
        string PlanName,
        DateTime StartedAt,
        DateTime? ExpiresAt,
        DateTime UpdatedAt,
        bool IsActive);

    public record PaymentDTO(
        int Id,
        int PlanId,
        string PlanName,
        decimal Amount,
        int CurrencyId,
        string CurrencyCode,
        int Status,
        string? ProviderPaymentId,
        DateTime? PaidAt,
        DateTime CreatedAt);

    public record CreatePaymentRequest(int PlanId, int CurrencyId, string? ProviderPaymentId);

    /// <summary>
    /// Текущее использование лимитов тарифа.
    /// Клиент использует, чтобы показать/заблокировать кнопки.
    /// </summary>
    public record UsageDTO(
        string PlanName,
        int ActiveVacancies,
        int? MaxActiveVacancies,
        int UploadsThisMonth,
        int? MaxUploadsPerMonth,
        bool CanParse,
        bool CanExportExcel,
        bool HasFullDashboard);
}
