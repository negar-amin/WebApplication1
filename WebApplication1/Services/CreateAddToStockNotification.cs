using WebApplication1.Data.Enums;
using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public class CreateAddToStockNotification : ICreateAddToStockNotification
	{
		private readonly IUserService _userService;
        public CreateAddToStockNotification(IUserService userService)
        {
			_userService = userService;
        }
        public async Task<IEnumerable<Notification>> CreateNotification(string productName, int productCount)
		{

			var users = await _userService.GetUsersWithRoles(Role.admin);
			var notifications = new List<Notification>();
			foreach (var user in users)
			{
				Notification notification = new Notification();
				notification.UserId = user.Id;
				notification.Title = $"Added to {productName} stock";
				notification.Description = $"Number of product added: {productCount}";
				notifications.Add(notification);
				
			}
			return  notifications;
		}
	}
}
