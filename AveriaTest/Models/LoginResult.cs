namespace AveriaTest.Models
{
    public class LoginResult
    {
        public static LoginResult GetSuccess => new LoginResult(LoginReason.Success);

        public bool Success => Reason == LoginReason.Success;

        public LoginReason Reason { get; }

        public LoginResult(LoginReason reason)
        {
            Reason = reason;
        }
    }
}
