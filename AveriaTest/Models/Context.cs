using System.Collections.Generic;

namespace AveriaTest.Models
{
    public class Context
    {
        public LoginReason Reason { get; set; }
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
    }
}
