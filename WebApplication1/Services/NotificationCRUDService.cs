using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public class NotificationCRUDService : INotificationCRUD
	{
		private readonly ICRUDRepository<Notification> _repository;
        public NotificationCRUDService(ICRUDRepository<Notification> NotificationRepository)
        {
            _repository = NotificationRepository;
        }
        public async Task AddNotificationAsync(Notification notification)
		{
			await _repository.AddAsync(notification);
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
			return await _repository.GetByIdAsync(id);
		}
	}
}
