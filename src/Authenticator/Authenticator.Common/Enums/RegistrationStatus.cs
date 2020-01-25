namespace Authenticator.Common
{
    public enum RegistrationStatus : byte
    {
        /// <summary>
        /// The failed.
        /// </summary>
        Failed,

        /// <summary>
        /// The email exists.
        /// </summary>
        EmailAlreadyInUse,

        /// <summary>
        /// The created.
        /// </summary>
        Created
    }
}