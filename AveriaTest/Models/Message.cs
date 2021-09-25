using System;

namespace AveriaTest.Models
{
    public class Message
    {
        public DateTime Date { get; }
        public string Text { get; }
        public User User { get; }

        public Message(string text, User user)
        {
            Date = DateTime.Now;
            Text = text;
            User = user;
        }
    }
}
