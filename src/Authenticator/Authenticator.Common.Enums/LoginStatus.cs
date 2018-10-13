namespace Authenticator.Common.Enums
{
    public enum LoginStatus : byte
    {
        /// <summary>
        /// The wrong email.
        /// </summary>
        WrongEmail,

        /// <summary>
        /// The wrong password.
        /// </summary>
        WrongPassword,

        /// <summary>
        /// The succeed.
        /// </summary>
        Succeed
    }
}