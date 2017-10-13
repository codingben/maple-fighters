using System;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Database.Common.AccessToken;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace Chat.Application.PeerLogic.Operations
{
    internal class AuthenticationOperationHandler : IOperationRequestHandler<AuthenticateRequestParameters, EmptyParameters>
    {
        private readonly Action onAuthenticated;
        private readonly Action onUnauthenticated;
        private readonly DatabaseAccessTokenExistence databaseAccessTokenExistence;

        public AuthenticationOperationHandler(Action onAuthenticated, Action onUnauthenticated)
        {
            this.onAuthenticated = onAuthenticated;
            this.onUnauthenticated = onUnauthenticated;

            databaseAccessTokenExistence = Server.Entity.Container.GetComponent<DatabaseAccessTokenExistence>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<AuthenticateRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var accessToken = messageData.Parameters.AccessToken;
            if (!databaseAccessTokenExistence.Exists(accessToken))
            {
                onUnauthenticated.Invoke();
                return null;
            }

            onAuthenticated.Invoke();
            return new EmptyParameters();
        }
    }
}