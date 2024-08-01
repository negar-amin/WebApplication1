using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public interface INotificationCRUD
	{
		Task<Notification> GetNotificationByIdAsync(int id);
		Task<IEnumerable<Notification>> GetAllNotificationsAsync();
		Task AddNotificationAsync(Notification notification);
		Task DeleteNotificationAsync(int id);
	}
}
