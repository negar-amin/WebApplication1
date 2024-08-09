using WebApplication1.Data.Entities;
using WebApplication1.Data.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Mapster;
using HotChocolate.Authorization;
using WebApplication1.Data.Enums;
using Microsoft.OpenApi.Extensions;
using WebApplication1.Data.Enums;
using WebApplication1.Services.Contracts;
using WebApplication1.GraphQL.GraphQLResponseSchema;

namespace WebApplication1.GraphQL.Mutation
{
    [ExtendObjectType(typeof(Mutation))]
	[Authorize]
	public class UserMutations
	{
		[AllowAnonymous]
		public async Task<Response<User>> AddUser(AddUserDTO input, [Service] IUserService UserService)
		{
			var response = await UserService.AddUserAsync(input);
			return response;
		}
		[Authorize(Roles = new[] { nameof(Role.admin), nameof(Role.staff), nameof(Role.customer) })]
		public async Task<Response<User>> UpdateUser(UpdateUserDTO input, [Service] IUserService UserService)
		{
			var response = await UserService.UpdateUserAsync(input);
			return response;
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
