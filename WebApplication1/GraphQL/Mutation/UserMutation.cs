using WebApplication1.Data.Models;
using WebApplication1.Services;
using WebApplication1.Data.DTO;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApplication1.GraphQL.Mutation
{
	[ExtendObjectType(Name = "Mutation")]
	public class UserMutation
	{
		public async Task<User> AddUser(AddUserDTO input, [Service] IUserService UserService)
		{
			var user = new User
			{
				FirstName = input.Name,
				LastName = input.LastName,
				Role = input.Role,
			};
			await UserService.AddUserAsync(user);
			return user;
		}
		public async Task<User> UpdateUser(int id, UpdateUserDTO input, [Service] IUserService UserService)
		{
			User user = await UserService.GetUserByIdAsync(id);
			if(user == null)
			{
				throw new Exception("User not found");
			}
			user.FirstName = input.Name == null? user.FirstName : input.Name;
			user.LastName = input.LastName == null? user.LastName : input.LastName;
			user.Role = (Role)(input.Role == null ? user.Role : input.Role);
			await UserService.UpdateUserAsync(user);
			return user;
		}
		public async Task<bool> DeleteUser(int id, [Service] IUserService UserService)
		{
			await UserService.DeleteUserAsync(id);
			return true;
		}
	}
}
