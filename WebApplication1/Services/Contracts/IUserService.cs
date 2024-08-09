using WebApplication1.Data.DTO;
using WebApplication1.Data.Enums;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.GraphQLResponseSchema;

namespace WebApplication1.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<Response<User>> AddUserAsync(AddUserDTO user);
        Task DeleteUserAsync(int id);
        Task<Response<User>> UpdateUserAsync(UpdateUserDTO input);
        Task<bool> AddDefaultValueToRole();
        Task<string> Login(string userName, string password);
        Task<User> GetCurrentUser();
		Task<Response<List<Order>>> GetCurrentUserOrders();
        Task<IEnumerable<User>> GetUsersWithRoles(Role role);
    }
}
