namespace InterestManagement
{
    public interface ILogger
    {
        /// <summary>
        /// Logs an information message.
        /// </summary>
        /// <param name="message">The message of the information.</param>
        void Info(string message);
    }
}