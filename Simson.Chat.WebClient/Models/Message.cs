using System;

namespace Simson.Chat.Models
{
    public class Message
    {
        public ulong Id { get; set; }
        public DateTime Date { get; } = DateTime.Now;
        public string Text { get; set; }
        public User User { get; set; }
    }
}
