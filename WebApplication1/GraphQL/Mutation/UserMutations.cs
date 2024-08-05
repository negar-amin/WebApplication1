using WebApplication1.Data.Entities;
using WebApplication1.Data.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Mapster;
using HotChocolate.Authorization;
using WebApplication1.Data.Enums;
using Microsoft.OpenApi.Extensions;
using WebApplication1.Data.Enums;
using WebApplication1.Services.Contracts;

namespace WebApplication1.GraphQL.Mutation
{
    [ExtendObjectType(typeof(Mutation))]
	[Authorize]
	public class UserMutations
	{
		[AllowAnonymous]
		public async Task<User> AddUser(AddUserDTO input, [Service] IUserService UserService)
		{
			var user = await UserService.AddUserAsync(input);
			return user;
		}
		[Authorize(Roles = new[] { nameof(Role.admin), nameof(Role.staff), nameof(Role.customer) })]
		public async Task<User> UpdateUser(UpdateUserDTO input, [Service] IUserService UserService)
		{
			var user = await UserService.UpdateUserAsync(input);
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
