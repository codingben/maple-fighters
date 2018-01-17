using CommonCommunicationInterfaces;
using CommonTools.Log;
using Registration.Application.Components;
using Registration.Common;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;

namespace Registration.Application.PeerLogic.Operations
{
    internal class RegisterOperationHandler : IOperationRequestHandler<RegisterRequestParameters, RegisterResponseParameters>
    {
        private readonly IDatabaseUserCreator databaseUserCreator;
        private readonly IDatabaseUserEmailVerifier databaseUserEmailVerifier;

        public RegisterOperationHandler()
        {
            databaseUserCreator = Server.Entity.GetComponent<IDatabaseUserCreator>().AssertNotNull();
            databaseUserEmailVerifier = Server.Entity.GetComponent<IDatabaseUserEmailVerifier>().AssertNotNull();
        }

        public RegisterResponseParameters? Handle(MessageData<RegisterRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var email = messageData.Parameters.Email;
            if (databaseUserEmailVerifier.Verify(email))
            {
                return new RegisterResponseParameters(RegisterStatus.EmailExists);
            }

            var password = messageData.Parameters.Password;
            var firstName = messageData.Parameters.FirstName;
            var lastName = messageData.Parameters.LastName;

            databaseUserCreator.Create(email, password, firstName, lastName);

            return new RegisterResponseParameters(RegisterStatus.Succeed);
        }
    }
}