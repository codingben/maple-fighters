namespace Authenticator.Domain.Aggregates.User.Services
{
    public enum AccountCreationStatus
    {
        /// <summary>
        /// The succeed.
        /// </summary>
        Succeed,

        /// <summary>
        /// The email exists.
        /// </summary>
        EmailExists
    }
}