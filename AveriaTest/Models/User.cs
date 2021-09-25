namespace AveriaTest.Models
{
    public class User
    {
        public string Name { get; }
        public UserStatus Status { get; set; }

        public User(string name, UserStatus status)
        {
            Name = name;
            Status = status;
        }
    }
}
