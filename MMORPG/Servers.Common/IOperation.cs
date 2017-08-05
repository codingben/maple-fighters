using System.Collections.Generic;

namespace Shared.Servers.Common
{
    /// <summary>
    /// An interface which a handler class of an operation should implement.
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Handle() is getting a request parameters and giving a response parameters.
        /// The following type is: (ResponseParameters : IParameters) Handle((RequestParameters : IParameters))
        /// </summary>
        /// <returns>It should return response parameters or null, which meaning that there are no parameters to send back.</returns>
        Dictionary<byte, object> OperationHandler(Dictionary<byte, object> parameters);
    }
}