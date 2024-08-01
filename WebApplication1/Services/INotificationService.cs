using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public interface INotificationService
	{
		Task SendNotification(string topic, Notification notification);
	}
}
