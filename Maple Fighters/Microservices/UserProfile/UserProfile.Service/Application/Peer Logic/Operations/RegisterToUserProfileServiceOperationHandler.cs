using System;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;
using UserProfile.Server.Common;

namespace UserProfile.Service.Application.PeerLogic.Operations
{
    internal class RegisterToUserProfileServiceOperationHandler : IOperationRequestHandler<RegisterToUserProfileServiceRequestParameters, EmptyParameters>
    {
        private readonly Action<int> onRegistered;

        public RegisterToUserProfileServiceOperationHandler(Action<int> onRegistered)
        {
            this.onRegistered = onRegistered;
        }

        public EmptyParameters? Handle(MessageData<RegisterToUserProfileServiceRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var serverId = messageData.Parameters.ServerId;
            onRegistered?.Invoke(serverId);
            return null;
        }
    }
}