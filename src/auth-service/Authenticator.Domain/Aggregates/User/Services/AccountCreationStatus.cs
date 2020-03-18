namespace Authenticator.Domain.Aggregates.User.Services
{
    public enum AccountCreationStatus
    {
        /// <summary>
        /// The failed.
        /// </summary>
        Failed,

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