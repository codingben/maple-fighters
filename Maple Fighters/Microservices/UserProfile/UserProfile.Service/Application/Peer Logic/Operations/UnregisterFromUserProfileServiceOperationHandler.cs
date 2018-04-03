using CommonCommunicationInterfaces;
using CommonTools.Log;
using ServerCommunicationHelper;
using UserProfile.Server.Common;
using UserProfile.Service.Application.Components.Interfaces;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    using Server = ServerApplication.Common.ApplicationBase.Server;

    internal class UnregisterFromUserProfileServiceOperationHandler : IOperationRequestHandler<UnregisterFromUserProfileServiceRequestParameters, EmptyParameters>
    {
        private readonly IServerIdToPeerIdConverter serverIdToPeerIdConverter;

        public UnregisterFromUserProfileServiceOperationHandler()
        {
            serverIdToPeerIdConverter = Server.Components.GetComponent<IServerIdToPeerIdConverter>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<UnregisterFromUserProfileServiceRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var serverId = messageData.Parameters.ServerId;
            serverIdToPeerIdConverter.Remove(serverId);
            return null;
        }
    }
}