using HotChocolate.Subscriptions;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using WebApplication1.Data.Enums;
using WebApplication1.Data.Entities;
using WebApplication1.Services.Contracts;

namespace WebApplication1.Services
{
    public class NotificationService : INotificationService
	{
		private readonly ITopicEventSender _eventSender;
		private readonly IUserService _userService;
		private readonly ICRUDRepository<Notification> _repository;

		public NotificationService(ITopicEventSender eventSender,IUserService userService, ICRUDRepository<Notification> repository)
		{
			_eventSender = eventSender;
			_userService = userService;
			_repository = repository;
		}

		public async Task AddToDBNotificationAsync(Notification notification)
		{
			await _repository.AddAsync(notification);
		}

		public async Task<IEnumerable<Notification>> CreateAddToStockNotification(string productName, int productCount)
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
			return notifications; 
		}

		public async Task DeleteNotificationAsync(int id)
		{
			await _repository.DeleteAsync(id);
		}

		public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
		{
			return await _repository.GetAllAsync();
		}

		public async Task<Notification> GetNotificationByIdAsync(int id)
		{
			return await _repository.GetByIdAsync(id); ;
		}

		public async Task SendNotification(string topic, List<Notification> notifications)
		{
            foreach (var notification in notifications)
            {
				await _eventSender.SendAsync(topic, notification);
				await AddToDBNotificationAsync(notification);
            }
        }

	}
}
