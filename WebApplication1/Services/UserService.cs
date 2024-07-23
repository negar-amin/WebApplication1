using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySqlConnector;
using System.Text;
using WebApplication1.Data.DTO;
using WebApplication1.Data.FluentValidation;
using WebApplication1.Data.Models;

namespace WebApplication1.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> _userRepository;
		private readonly TokenService _tokenService;
        public UserService(IRepository<User> userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
			_tokenService = tokenService;
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

		public async Task UpdateUserAsync(User user)
		{
			await _userRepository.UpdateAsync(user);
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
			return _tokenService.GenerateToken(userName);
		}
	}
}
