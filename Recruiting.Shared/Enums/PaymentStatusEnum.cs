namespace Recruiting.Shared.Enums
{
    public enum PaymentStatusEnum
    {
        Pending = 1,      // Ожидает оплату
        Paid = 2,         // Оплачено
        Failed = 3,       // Ошибка оплаты
        Refunded = 4      // Возврат
    }
}
