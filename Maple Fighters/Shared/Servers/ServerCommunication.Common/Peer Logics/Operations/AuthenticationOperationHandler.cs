using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;

namespace ServerCommunication.Common
{
    public class AuthenticationOperationHandler : IOperationRequestHandler<AuthenticateRequestParameters, AuthenticateResponseParameters>
    {
        private readonly string secretKey;
        private readonly Action onAuthenticated;
        private readonly Action onNonAuthenticated;

        public AuthenticationOperationHandler(string secretKey, Action onAuthenticated, Action onNonAuthenticated)
        {
            this.secretKey = secretKey.AssertNotNull(MessageBuilder.Trace("Secret key not found."));
            this.onAuthenticated = onAuthenticated;
            this.onNonAuthenticated = onNonAuthenticated;
        }

        public AuthenticateResponseParameters? Handle(MessageData<AuthenticateRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var receivedSecretKey = messageData.Parameters.SecretKey;
            if (receivedSecretKey == secretKey)
            {
                onAuthenticated.Invoke();
                return new AuthenticateResponseParameters(AuthenticationStatus.Succeed);
            }

            onNonAuthenticated.Invoke();
            return null;
        }
    }
}