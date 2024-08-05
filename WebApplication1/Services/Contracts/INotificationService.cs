using WebApplication1.Data.Entities;

namespace WebApplication1.Services.Contracts
{
    public interface INotificationService
    {
        Task SendNotification(string topic, List<Notification> notification);
        Task<Notification> GetNotificationByIdAsync(int id);
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task AddToDBNotificationAsync(Notification notifications);
        Task DeleteNotificationAsync(int id);
        Task<IEnumerable<Notification>> CreateAddToStockNotification(string productName, int productCount);
    }
}
