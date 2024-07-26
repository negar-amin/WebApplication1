using HotChocolate.Authorization;
using WebApplication1.Data.Models;
using WebApplication1.Services;
using WebApplication1.Data.Enums;
using Microsoft.OpenApi.Extensions;

namespace WebApplication1.GraphQL.Query
{
	
	[ExtendObjectType(Name = "Query")]
	[Authorize(Roles = new[] {nameof(Role.admin),nameof(Role.staff)})]
	public class UserQuery
	{
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
		[AllowAnonymous]
		public async Task<string> UserLogin(string userName, string Password, [Service]IUserService userService)
		{
			return await userService.Login(userName, Password);
		}
	}
}
