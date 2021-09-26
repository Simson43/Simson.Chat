using System.Collections.Generic;

namespace Simson.Chat.Models
{
    public class Context
    {
        public LoginReason Reason { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
    }
}
