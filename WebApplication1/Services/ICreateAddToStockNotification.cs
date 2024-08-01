using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public interface ICreateAddToStockNotification
	{
		Task<IEnumerable<Notification>> CreateNotification(string productName, int productCount);
	}
}
