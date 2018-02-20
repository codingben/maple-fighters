using System;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Database.Common.AccessToken;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace CharactersService.Application.PeerLogics.Operations
{
    internal class AuthenticationOperationHandler : IOperationRequestHandler<AuthenticateRequestParameters, AuthenticateResponseParameters>
    {
        private readonly int peerId;
        private readonly Action<int> onAuthenticated;
        private readonly ILocalDatabaseAccessTokens databaseAccessTokens;
        private readonly IDatabaseAccessTokenExistence databaseAccessTokenExistence;
        private readonly IDatabaseAccessTokenProvider databaseAccessTokenProvider;
        private readonly IDatabaseUserIdViaAccessTokenProvider databaseUserIdViaAccessTokenProvider;

        public AuthenticationOperationHandler(int peerId, Action<int> onAuthenticated)
        {
            this.peerId = peerId;
            this.onAuthenticated = onAuthenticated;

            databaseAccessTokens = Server.Components.GetComponent<ILocalDatabaseAccessTokens>().AssertNotNull();
            databaseAccessTokenExistence = Server.Components.GetComponent<IDatabaseAccessTokenExistence>().AssertNotNull();
            databaseAccessTokenProvider = Server.Components.GetComponent<IDatabaseAccessTokenProvider>().AssertNotNull();
            databaseUserIdViaAccessTokenProvider = Server.Components.GetComponent<IDatabaseUserIdViaAccessTokenProvider>().AssertNotNull();
        }

        public AuthenticateResponseParameters? Handle(MessageData<AuthenticateRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var accessToken = messageData.Parameters.AccessToken;

            if (databaseAccessTokens.Exists(accessToken))
            {
                return new AuthenticateResponseParameters(AuthenticationStatus.Failed);
            }

            if (!databaseAccessTokenExistence.Exists(accessToken))
            {
                return new AuthenticateResponseParameters(AuthenticationStatus.Failed);
            }

            var userId = databaseUserIdViaAccessTokenProvider.GetUserId(accessToken);

            if (databaseAccessTokenProvider.GetAccessToken(userId) != accessToken)
            {
                return new AuthenticateResponseParameters(AuthenticationStatus.Failed);
            }

            databaseAccessTokens.Add(peerId, accessToken);

            onAuthenticated.Invoke(userId);
            return new AuthenticateResponseParameters(AuthenticationStatus.Succeed);
        }
    }
}