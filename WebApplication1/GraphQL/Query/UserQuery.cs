using HotChocolate.Authorization;
using WebApplication1.Data.Models;
using WebApplication1.Services;

namespace WebApplication1.GraphQL.Query
{
	[ExtendObjectType(Name ="Query")]
	public class UserQuery
	{
		//[Authorize]
		public async Task<IEnumerable<User>> GetAllUsers([Service] IUserService userService)
		{
			var users = await userService.GetAllUsersAsync();
			return users;
		}
		public async Task<User> GetUserById(int id, [Service] IUserService userService)
		{
			var user = await userService.GetUserByIdAsync(id);
			return user;
		}
		public async Task<string> UserLogin(string userName, string Password, [Service]IUserService userService)
		{
			return await userService.Login(userName, Password);
		}
	}
}
