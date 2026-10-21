namespace Recruiting.Shared.Enums
{
    public enum ParseJobStatusEnum
    {
        Pending = 1,           // Ожидает
        Running = 2,           // В работе
        Completed = 3,         // Готово
        Failed = 4,            // Ошибка
        NeedsManualReview = 5  // Требует ручной проверки
    }
}
