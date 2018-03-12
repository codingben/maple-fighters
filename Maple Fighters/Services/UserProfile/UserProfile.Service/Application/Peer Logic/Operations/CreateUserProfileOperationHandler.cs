using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class CreateUserProfileOperationHandler : IOperationRequestHandler<CreateUserProfileRequestParameters, CreateUserProfileResponseParameters>
    {
        private readonly IDatabaseUserProfileExistence databaseUserProfileExistence;
        private readonly IDatabaseUserProfileCreator databaseUserProfileCreator;

        public CreateUserProfileOperationHandler()
        {
            databaseUserProfileExistence = Server.Components.GetComponent<IDatabaseUserProfileExistence>().AssertNotNull();
            databaseUserProfileCreator = Server.Components.GetComponent<IDatabaseUserProfileCreator>().AssertNotNull();
        }

        public CreateUserProfileResponseParameters? Handle(MessageData<CreateUserProfileRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var userId = messageData.Parameters.UserId;

            if (databaseUserProfileExistence.Exists(userId))
            {
                return new CreateUserProfileResponseParameters();
            }

            databaseUserProfileCreator.Create(userId);
            return new CreateUserProfileResponseParameters(UserProfileCreationStatus.Succeed);
        }
    }
}