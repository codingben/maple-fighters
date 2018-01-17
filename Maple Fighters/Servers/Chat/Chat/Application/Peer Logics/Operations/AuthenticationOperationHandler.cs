using System;
using Chat.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Database.Common.AccessToken;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace Chat.Application.PeerLogic.Operations
{
    internal class AuthenticationOperationHandler : IOperationRequestHandler<AuthenticateRequestParameters, AuthenticateResponseParameters>
    {
        private readonly int peerId;
        private readonly Action onAuthenticated;
        private readonly ILocalDatabaseAccessTokens databaseAccessTokens;
        private readonly IDatabaseAccessTokenExistence databaseAccessTokenExistence;

        public AuthenticationOperationHandler(int peerId, Action onAuthenticated)
        {
            this.peerId = peerId;
            this.onAuthenticated = onAuthenticated;

            databaseAccessTokens = Server.Entity.GetComponent<ILocalDatabaseAccessTokens>().AssertNotNull();
            databaseAccessTokenExistence = Server.Entity.GetComponent<IDatabaseAccessTokenExistence>().AssertNotNull();
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

            databaseAccessTokens.Add(peerId, accessToken);

            onAuthenticated.Invoke();
            return new AuthenticateResponseParameters(AuthenticationStatus.Succeed);
        }
    }
}