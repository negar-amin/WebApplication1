using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Data.DTO;

namespace WebApplication1.Data.FluentValidation
{
	public class AddUserInputValidator : AbstractValidator<AddUserDTO>
	{
        public AddUserInputValidator()
        {
            RuleFor(u => u.UserName).NotEmpty();
            RuleFor(u => u.Password)
                .MinimumLength(8)
                .Matches("^(?=.*[0-9]).+$").WithMessage("password must contain numbers")
                .Matches("(?=.*[a-z]).+$").WithMessage("password must contain lowercase chracters")
                .Matches("(?=.*[A-Z]).+$").WithMessage("password must contain uppercase chracters")
                .Matches("(?=\\S+$).+$").WithMessage("password must contain no white spaces");
        }
    }
}
