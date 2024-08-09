using HotChocolate.Authorization;
using WebApplication1.Data.Entities;
using WebApplication1.Data.Enums;
using Microsoft.OpenApi.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApplication1.Services.Contracts;
using WebApplication1.GraphQL.GraphQLResponseSchema;

namespace WebApplication1.GraphQL.Query
{

    [ExtendObjectType(typeof(Query))]
	[Authorize(Roles = new[] {nameof(Role.admin),nameof(Role.staff)})]
	public class UserQueries
	{
		[AllowAnonymous]
		public async Task<Response<List<Order>>> GetCurrentUserOrders([Service] IUserService userService)
		{
			return await userService.GetCurrentUserOrders();
		}
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
		[AllowAnonymous]
		public async Task<User> GetCurrentUser([Service]IUserService userService)
		{
			User user = await userService.GetCurrentUser();
			return user;
		}
	}
}
