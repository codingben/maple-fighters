namespace Authenticator.Domain.Aggregates.User.Services
{
    public enum AuthenticationStatus : byte
    {
        /// <summary>
        /// The failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The authenticated.
        /// </summary>
        Authenticated,

        /// <summary>
        /// The not found.
        /// </summary>
        NotFound,

        /// <summary>
        /// The wrong password.
        /// </summary>
        WrongPassword
    }
}