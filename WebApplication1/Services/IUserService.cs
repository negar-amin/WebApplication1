using WebApplication1.Data.DTO;
using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAllUsersAsync();
		Task<User> GetUserByIdAsync(int id);
		Task<User> AddUserAsync(AddUserDTO user);
		Task DeleteUserAsync(int id);
		Task UpdateUserAsync(User user);
		Task<bool> AddDefaultValueToRole();
		Task<string> Login(string userName, string password);
	}
}
