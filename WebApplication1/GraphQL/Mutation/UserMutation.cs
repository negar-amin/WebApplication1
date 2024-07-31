using WebApplication1.Data.Models;
using WebApplication1.Services;
using WebApplication1.Data.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Mapster;
using HotChocolate.Authorization;
using WebApplication1.Data.Enums;
using Microsoft.OpenApi.Extensions;
using WebApplication1.Data.Enums;

namespace WebApplication1.GraphQL.Mutation
{
	[ExtendObjectType(Name = "Mutation")]
	[Authorize]
	public class UserMutation
	{
		string admin = Role.customer.GetDisplayName();
		public async Task<User> AddUser(AddUserDTO input, [Service] IUserService UserService)
		{
			var user = await UserService.AddUserAsync(input);
			return user;
		}
		[Authorize(Roles = new[] { nameof(Role.admin), nameof(Role.staff), nameof(Role.customer) })]
		public async Task<User> UpdateUser(UpdateUserDTO input, [Service] IUserService UserService)
		{
			User user = await UserService.GetCurrentUser();

			if (user == null)
			{
				throw new Exception("User not found");
			}
			input.Adapt(user);
			await UserService.UpdateUserAsync(user);
			return user;
		}
		[Authorize(Roles = new[] { nameof(Role.admin)})]
		public async Task<bool> DeleteUser(int id, [Service] IUserService UserService)
		{
			await UserService.DeleteUserAsync(id);
			return true;
		}
		[Authorize(Roles =new[]{nameof(Role.admin)})]
		public async Task<bool> AddDefaultValueToRole([Service] IUserService userService)
		{
			return await userService.AddDefaultValueToRole();
		}

	}
}
