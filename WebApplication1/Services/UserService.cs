using Microsoft.EntityFrameworkCore;
using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddUserAsync(User user)
		{
			await _userRepository.AddAsync(user);
		}

		public async Task DeleteUserAsync(int id)
		{
			await _userRepository.DeleteAsync(id);
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			return await _userRepository.GetAllAsync();
		}

		public async Task<User> GetUserByIdAsync(int id)
		{
			return await _userRepository.GetByIdAsync(id);
		}

		public async Task UpdateUserAsync(User user)
		{
			await _userRepository.UpdateAsync(user);
		}
	}
}
