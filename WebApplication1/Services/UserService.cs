using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySqlConnector;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data.DTO;
using WebApplication1.Data.Enums;
using WebApplication1.Data.FluentValidation;
using WebApplication1.Data.Entities;
using WebApplication1.GraphQL.Subscription;
using WebApplication1.Repositories.Contracts;
using WebApplication1.Services.Contracts;

namespace WebApplication1.Services
{
    public class UserService : IUserService
	{
		private readonly ICRUDRepository<User> _userRepository;
		private readonly TokenService _tokenService;
		private readonly IConfiguration _config;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IOrderQueryRepository _orderQueryRepository;
		public UserService(ICRUDRepository<User> userRepository, TokenService tokenService, IConfiguration configuration,IHttpContextAccessor contextAccessor, IOrderQueryRepository orderQueryRepository)
        {
            _userRepository = userRepository;
			_tokenService = tokenService;
			_config = configuration;
			_contextAccessor = contextAccessor;
			_orderQueryRepository = orderQueryRepository;
        }
		private string GenerateSalt()
		{
			return Guid.NewGuid().ToString();
		}

		private string HashPassword(string password, string salt)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(password + salt));
		}
		public async Task<User> AddUserAsync(AddUserDTO userInput)
		{
			var validator = new AddUserInputValidator();
			validator.ValidateAndThrow(userInput);
			var salt = GenerateSalt();
			var passwordHash = HashPassword(userInput.Password,salt);
			User user = new User{
				PasswordHash = passwordHash,
				PasswordSalt = salt
			};
			userInput.Adapt(user);
			try
			{
				await _userRepository.AddAsync(user);
			}
			catch (DbUpdateException ex)
			{
				if (ex.InnerException is MySqlException mySqlException && mySqlException.Number == 1062)
				{
					throw new Exception("UserName already exists.");
				}
			}
			return user;
		}

		public async Task DeleteUserAsync(int id)
		{
			await _userRepository.DeleteAsync(id);
		}

		public async Task<IEnumerable<User>> GetAllUsersAsync()
		{
			return await _userRepository.GetAllAsync();
		}

		public async Task<User> GetUserByIdAsync(int id)
		{
			return await _userRepository.GetByIdAsync(id);
		}

		public async Task<User> UpdateUserAsync(UpdateUserDTO input)
		{
			User user = await GetCurrentUser();

			if (user == null)
			{
				throw new Exception("User not found");
			}
			input.Adapt(user);
			await _userRepository.UpdateAsync(user);
			return user;
		}
		public async Task<bool> AddDefaultValueToRole()
		{
			var users = await _userRepository.GetAllAsync();
            foreach (var user in users)
            {
				user.Role = user.Role + 1;
				await _userRepository.UpdateAsync(user);
            }
			return true;
        }

		public async Task<string> Login(string userName, string password)
		{
			var users = await _userRepository.GetAllAsync();
			var user = users.FirstOrDefault(user => user.UserName == userName);
			if (user == null)
				throw new Exception($"{userName} is not registered.");
			var passwordHash = HashPassword(password, user.PasswordSalt);
			if (passwordHash != user.PasswordHash)
				throw new Exception("Password is incorrect.");
			return _tokenService.GenerateToken(user);
		}
		private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
		{

			var tokenHandler = new JwtSecurityTokenHandler();
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("JwtSettings").GetValue<string>("SecretKey"))), // Provide your signing key
				ValidateIssuer = false, // Set to true if you want to validate the issuer
				ValidateAudience = false // Set to true if you want to validate the audience
			};

			try
			{
				// Validate token, and then extract claims
				ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
				return principal;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}

		public async Task<User> GetCurrentUser()
		{
			var context = _contextAccessor.HttpContext;
			var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
			string token = authHeader?.Split(' ').Last();
			if (token != null)
			{
				ClaimsPrincipal claims = GetClaimsPrincipalFromToken(token);
				IEnumerable<User> users = await GetAllUsersAsync();
				if (claims == null)
					throw new Exception("Unauthorized");
				else
				{
					var userName = claims.FindFirst(ClaimTypes.Name)?.Value;
					User user = users.FirstOrDefault(u => u.UserName == userName);
					return user;
				}
			}
			else
				throw new Exception("empty token exception");

		}

		public async Task<ICollection<Order>> GetCurrentUserOrders()
		{
			User user = await GetCurrentUser();
			ICollection<Order> orders =  _orderQueryRepository.GetOrdersByUserId(user.Id);
			return orders;
		}

		public async Task<IEnumerable<User>> GetUsersWithRoles(Role role)
		{
			var users = await GetAllUsersAsync();
			var usersWithDesiredRole = users.Where(user => user.Role == role);
			return usersWithDesiredRole;
		}
	}
}
