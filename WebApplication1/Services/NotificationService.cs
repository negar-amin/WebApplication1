using HotChocolate.Subscriptions;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using WebApplication1.Data.Enums;
using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public class NotificationService:INotificationService
	{
		private readonly ITopicEventSender _eventSender;
		private readonly IUserService _userService;

		public NotificationService(ITopicEventSender eventSender,IUserService userService)
		{
			_eventSender = eventSender;
			_userService = userService;
		}



		public async Task SendNotification(string topic, Notification notification)
		{
				await _eventSender.SendAsync(topic, notification);
        }

	}
}
