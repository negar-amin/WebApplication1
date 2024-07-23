using FluentValidation;
using WebApplication1.Data.Models;

namespace WebApplication1.Data.FluentValidation
{
	public class UserNameValidator: AbstractValidator<User>
	{
        public UserNameValidator()
        {
            RuleFor(u => u.UserName).NotEmpty().MinimumLength(1);
            RuleFor(u => u.PasswordHash).NotEmpty().MinimumLength(8);
        }
    }
}
