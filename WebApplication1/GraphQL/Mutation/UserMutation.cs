using WebApplication1.Data.Models;
using WebApplication1.Services;
using WebApplication1.Data.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Mapster;

namespace WebApplication1.GraphQL.Mutation
{
	[ExtendObjectType(Name = "Mutation")]
	public class UserMutation
	{
		public async Task<User> AddUser(AddUserDTO input, [Service] IUserService UserService)
		{
			var user = await UserService.AddUserAsync(input);
			return user;
		}
		public async Task<User> UpdateUser(int id, UpdateUserDTO input, [Service] IUserService UserService)
		{
			User user = await UserService.GetUserByIdAsync(id);

			if(user == null)
			{
				throw new Exception("User not found");
			}
			input.Adapt(user);
			await UserService.UpdateUserAsync(user);
			return user;
		}
		public async Task<bool> DeleteUser(int id, [Service] IUserService UserService)
		{
			await UserService.DeleteUserAsync(id);
			return true;
		}
		public async Task<bool> AddDefaultValueToRole([Service] IUserService userService)
		{
			return await userService.AddDefaultValueToRole();
		}

	}
}
