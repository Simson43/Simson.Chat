using AveriaTest.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AveriaTest
{
    public interface IChatContext
    {
        event Action<Message> MessageReceived;
        event Action<User> UserStatusChanged;

        Task<LoginResult> Login(string userName);
        Task Logout(string userName);
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<Message>> GetMessages();
        Task AddMessage(Message message);
    }
}
