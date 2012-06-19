using FluentValidation; 

namespace GestUAB.Models
{
    public interface IModel
    {

    }

    public class UserModel : IModel
    {
        public string Username { get; set; }

        public string FirstName { get; set; }


        public string LastName { get; set; }

        public string Email {get; set;}

        public string Name { get { return FirstName + " " + LastName; } }
    }


    public class UserModelValidator : AbstractValidator<UserModel> {
        public UserModelValidator ()
        {
            RuleSet("Names", () => {
                RuleFor(user => user.Username).NotEmpty();
                RuleFor(user => user.FirstName).NotEmpty();
                RuleFor(user => user.LastName).NotEmpty();
            });
            RuleFor(user => user.Email).NotEmpty().EmailAddress();
        }
    }

}