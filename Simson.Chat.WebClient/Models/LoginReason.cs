namespace Simson.Chat.Models
{
    public enum LoginReason
    {
        Success,
        AlreadyExists,
        IncorrectUserName,
        UnknownError = 1024,
    }
}
