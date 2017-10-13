using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Database.Common.AccessToken;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class AuthenticationOperationHandler : IOperationRequestHandler<AuthenticateRequestParameters, EmptyParameters>
    {
        private readonly Action<int> onAuthenticated;
        private readonly Action onUnauthenticated;
        private readonly DatabaseAccessTokenExistence databaseAccessTokenExistence;
        private readonly DatabaseAccessTokenProvider databaseAccessTokenProvider;
        private readonly DatabaseUserIdViaAccessTokenProvider databaseUserIdViaAccessTokenProvider;

        public AuthenticationOperationHandler(Action<int> onAuthenticated, Action onUnauthenticated)
        {
            this.onAuthenticated = onAuthenticated;
            this.onUnauthenticated = onUnauthenticated;

            databaseAccessTokenExistence = Server.Entity.Container.GetComponent<DatabaseAccessTokenExistence>().AssertNotNull();
            databaseAccessTokenProvider = Server.Entity.Container.GetComponent<DatabaseAccessTokenProvider>().AssertNotNull();
            databaseUserIdViaAccessTokenProvider = Server.Entity.Container.GetComponent<DatabaseUserIdViaAccessTokenProvider>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<AuthenticateRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var accessToken = messageData.Parameters.AccessToken;

            if (!databaseAccessTokenExistence.Exists(accessToken))
            {
                onUnauthenticated.Invoke();
                return null;
            }

            var userId = databaseUserIdViaAccessTokenProvider.GetUserId(accessToken);

            if (databaseAccessTokenProvider.GetAccessToken(userId) != accessToken)
            {
                onUnauthenticated.Invoke();
                return null;
            }

            onAuthenticated.Invoke(userId);
            return new EmptyParameters();
        }
    }
}