using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
		Task AddUserAsync(User user);
		Task DeleteUserAsync(int id);
		Task UpdateUserAsync(User user);
		Task<bool> AddDefaultValueToRole();
	}
}
