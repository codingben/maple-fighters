namespace Authenticator.Common.Enums
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