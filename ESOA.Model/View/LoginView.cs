namespace ESOA.Model.View
{
    public class LoginView
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnURL { get; set; }
    }

    public class LoginResult
    {
        public bool IsValidated { get; set; }
        public string ReturnURL { get; set; }
        public string Message { get; set; }
        public bool IsFirstTimeLogin { get; set; }
        public string CurrentUserId { get; set; }
    }
}
