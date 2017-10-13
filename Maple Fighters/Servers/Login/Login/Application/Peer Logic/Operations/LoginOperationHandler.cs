using CommonCommunicationInterfaces;
using CommonTools.Log;
using Database.Common.AccessToken;
using Login.Application.Components;
using Login.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace Login.Application.PeerLogic.Operations
{
    internal class LoginOperationHandler : IOperationRequestHandler<LoginRequestParameters, LoginResponseParameters>
    {
        private readonly DatabaseUserVerifier databaseUserVerifier;
        private readonly DatabaseUserPasswordVerifier databaseUserPasswordVerifier;
        private readonly DatabaseUserIdProvider databaseUserIdProvider;
        private readonly DatabaseAccessTokenCreator databaseAccessTokenCreator;
        private readonly DatabaseAccessTokenExistenceViaUserId databaseAccessTokenExistenceViaUserId;
        private readonly DatabaseAccessTokenProvider databaseAccessTokenProvider;

        public LoginOperationHandler()
        {
            databaseUserVerifier = Server.Entity.Container.GetComponent<DatabaseUserVerifier>().AssertNotNull();
            databaseUserPasswordVerifier = Server.Entity.Container.GetComponent<DatabaseUserPasswordVerifier>().AssertNotNull();
            databaseUserIdProvider = Server.Entity.Container.GetComponent<DatabaseUserIdProvider>().AssertNotNull();
            databaseAccessTokenCreator = Server.Entity.Container.GetComponent<DatabaseAccessTokenCreator>().AssertNotNull();
            databaseAccessTokenExistenceViaUserId = Server.Entity.Container.GetComponent<DatabaseAccessTokenExistenceViaUserId>().AssertNotNull();
            databaseAccessTokenProvider = Server.Entity.Container.GetComponent<DatabaseAccessTokenProvider>().AssertNotNull();
        }

        public LoginResponseParameters? Handle(MessageData<LoginRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var email = messageData.Parameters.Email;
            if (!databaseUserVerifier.IsExists(email))
            {
                return new LoginResponseParameters(LoginStatus.UserNotExist);
            }

            var password = messageData.Parameters.Password;
            if(!databaseUserPasswordVerifier.Verify(email, password))
            {
                return new LoginResponseParameters(LoginStatus.PasswordIncorrect);
            }

            var userId = databaseUserIdProvider.GetUserId(email);
            var accessToken = databaseAccessTokenExistenceViaUserId.Exists(userId) 
                ? databaseAccessTokenProvider.GetAccessToken(userId) : databaseAccessTokenCreator.Create(userId);

            return new LoginResponseParameters(LoginStatus.Succeed, accessToken, true);
        }
    }
}