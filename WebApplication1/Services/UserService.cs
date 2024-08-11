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
using WebApplication1.GraphQL.GraphQLResponseSchema;
using System;

namespace WebApplication1.Services
{
    public class UserService : IUserService
	{
		private readonly ICRUDRepository<User> _userRepository;
		private readonly TokenService _tokenService;
		private readonly IConfiguration _config;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IOrderRepository _orderRepository;
		public UserService(IOrderRepository orderRepository ,ICRUDRepository<User> userRepository, TokenService tokenService, IConfiguration configuration,IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
			_tokenService = tokenService;
			_config = configuration;
			_contextAccessor = contextAccessor;
			_orderRepository = orderRepository;
        }
		private string GenerateSalt()
		{
			return Guid.NewGuid().ToString();
		}

		private string HashPassword(string password, string salt)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(password + salt));
		}
		public async Task<Response<User>> AddUserAsync(AddUserDTO userInput)
		{
			var validator = new AddUserInputValidator();
			Response<User> response = new Response<User>();
			var validationResult = validator.Validate(userInput);
            foreach (var validationError in validationResult.Errors)
            {
				var fields = validationError.ErrorMessage.Split(":").ToList();
				ResponseError error = new ResponseError(int.Parse(fields[0]), fields[1]) with { Detail = fields[2] };
				response.Status.Add(error);

            }
			if (response.Status.Count > 0) return response;
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
					response.Status.Add(ResponseError.DuplicateUniqueProperty with { Detail="UserName already exist"});
				}
			}
			return response;
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

		public async Task<Response<User>> UpdateUserAsync(UpdateUserDTO input)
		{
			Response<User> response = new Response<User>();
			var user = await GetCurrentUser();
			input.Adapt(user.Data);
			await _userRepository.UpdateAsync(user.Data);
			response.Data = user.Data;
			return response;
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

		public async Task<Response<string>> Login(string userName, string password)
		{
			Response<string> response = new Response<string>();
			var users = await _userRepository.GetAllAsync();
			var user = users.FirstOrDefault(user => user.UserName == userName);
			if (user == null)
				response.Status.Add(ResponseError.NotFound with { Detail = $"no user with username {userName} was not found" });
			else
			{
				var passwordHash = HashPassword(password, user.PasswordSalt);
				if (passwordHash != user.PasswordHash)
					response.Status.Add(ResponseError.IncorrectPassword with { Detail = "password is incorrect" });
				if (response.Status.Count == 0) response.Data = _tokenService.GenerateToken(user);
			}
			return response;
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

		public async Task<Response<User>> GetCurrentUser()
		{
			Response<User> response = new Response<User>();
			var context = _contextAccessor.HttpContext;
			var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
			string token = authHeader?.Split(' ').Last();
			if (token != null)
			{
				try
				{
					ClaimsPrincipal claims = GetClaimsPrincipalFromToken(token);
					IEnumerable<User> users = await GetAllUsersAsync();
					if (claims == null)
						response.Status.Add(ResponseError.Unauthorized with { Detail="no claims found"});
					else
					{
						var userName = claims.FindFirst(ClaimTypes.Name)?.Value;
						User user = users.FirstOrDefault(u => u.UserName == userName);
						response.Data = user;
					}
				}
				catch (Exception ex)
				{
					response.Status.Add(ResponseError.Unauthorized);
				}
				
			}
			else
				response.Status.Add(ResponseError.EmptyToken);
			return response;

		}

		public async Task<Response<List<Order>>> GetCurrentUserOrders()
		{
			var user = await GetCurrentUser();
			var response =  _orderRepository.GetOrdersByUserId(user.Data.Id);
			return response;
		}

		public async Task<IEnumerable<User>> GetUsersWithRoles(Role role)
		{
			var users = await GetAllUsersAsync();
			var usersWithDesiredRole = users.Where(user => user.Role == role);
			return usersWithDesiredRole;
		}
	}
}
