using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Data.DTO;
using WebApplication1.GraphQL.GraphQLResponseSchema;

namespace WebApplication1.Data.FluentValidation
{
	public class AddUserInputValidator : AbstractValidator<AddUserDTO>
	{
        public AddUserInputValidator()
        {
            RuleFor(u => u.UserName).NotEmpty();
            RuleFor(u => u.Password)
                .MinimumLength(8).WithMessage($"{ResponseError.PasswordRquirement.ToString()}password must contain atleast 8 characters")
				.Matches("^(?=.*[0-9]).+$").WithMessage($"{ResponseError.PasswordRquirement.ToString()}password must contain numbers")
                .Matches("(?=.*[a-z]).+$").WithMessage($"{ResponseError.PasswordRquirement.ToString()}password must contain lowercase chracters")
                .Matches("(?=.*[A-Z]).+$").WithMessage($"{ResponseError.PasswordRquirement.ToString()}password must contain uppercase chracters")
                .Matches("(?=\\S+$).+$").WithMessage($"{ResponseError.PasswordRquirement.ToString()}password must contain no white spaces");
        }
    }
}
