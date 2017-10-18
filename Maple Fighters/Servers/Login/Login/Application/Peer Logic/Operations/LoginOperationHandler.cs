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
        private readonly IDatabaseUserVerifier databaseUserVerifier;
        private readonly IDatabaseUserPasswordVerifier databaseUserPasswordVerifier;
        private readonly IDatabaseUserIdProvider databaseUserIdProvider;
        private readonly IDatabaseAccessTokenCreator databaseAccessTokenCreator;
        private readonly IDatabaseAccessTokenExistenceViaUserId databaseAccessTokenExistenceViaUserId;
        private readonly IDatabaseAccessTokenProvider databaseAccessTokenProvider;

        public LoginOperationHandler()
        {
            databaseUserVerifier = Server.Entity.Container.GetComponent<IDatabaseUserVerifier>().AssertNotNull();
            databaseUserPasswordVerifier = Server.Entity.Container.GetComponent<IDatabaseUserPasswordVerifier>().AssertNotNull();
            databaseUserIdProvider = Server.Entity.Container.GetComponent<IDatabaseUserIdProvider>().AssertNotNull();
            databaseAccessTokenCreator = Server.Entity.Container.GetComponent<IDatabaseAccessTokenCreator>().AssertNotNull();
            databaseAccessTokenExistenceViaUserId = Server.Entity.Container.GetComponent<IDatabaseAccessTokenExistenceViaUserId>().AssertNotNull();
            databaseAccessTokenProvider = Server.Entity.Container.GetComponent<IDatabaseAccessTokenProvider>().AssertNotNull();
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