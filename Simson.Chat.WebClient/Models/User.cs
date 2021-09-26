namespace Simson.Chat.Models
{
    public class User
    {
        public ulong Id { get; }
        public string Name { get; set; }
        public UserStatus Status { get; set; }

        public User(string name, UserStatus status)
        {
            Name = name;
            Status = status;
        }
    }
}
