
using FluentValidation;

namespace GestUAB.Models
{
    public class User : IModel
    {
 
        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Name { get { return FirstName + " " + LastName; } }
    }

    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username).NotEmpty();
            RuleFor(user => user.Username).Length(5, 15);
            RuleFor(user => user.Username).Matches("[a-z]*");
            RuleFor(user => user.Email).NotEmpty().EmailAddress();
            RuleFor(user => user.FirstName).NotEmpty();
            RuleFor(user => user.LastName).NotEmpty();
        }
    }
}