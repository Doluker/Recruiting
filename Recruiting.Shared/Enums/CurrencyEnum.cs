using Recruiting.Shared.DTOs;
using System.Globalization;

namespace Recruiting.Shared.Enums
{
    public enum CurrencyEnum
    {
        Rub = 1, // Рубль
        Usd = 2,   // Доллар
        Eur = 3    // Евро
    }
    public static class CurrencyExtensions
    {
        public static string Symbol(this CurrencyEnum currency, IReadOnlyList<CurrencyDTO> currencies) => currencies.FirstOrDefault(c => c.Id == (int)currency)?.Symbol ?? string.Empty;

        /// <summary>990,00₽</summary>
        public static string Format(this CurrencyEnum currency, decimal amount, IReadOnlyList<CurrencyDTO> currencies) => amount.ToString("N2", new CultureInfo("ru-RU")) + currency.Symbol(currencies);
    }


}
