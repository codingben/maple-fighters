using CommonCommunicationInterfaces;
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

        public LoginOperationHandler()
        {
            databaseUserVerifier = Server.Entity.Container.GetComponent<DatabaseUserVerifier>();
            databaseUserPasswordVerifier = Server.Entity.Container.GetComponent<DatabaseUserPasswordVerifier>();
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

            return new LoginResponseParameters(LoginStatus.Succeed);
        }
    }
}