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

        public string Name { get { return FirstName + " " + LastName; } }
    }
}