using System;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Game.Application.PeerLogic.Operations
{
    internal class AuthenticationOperationHandler : IOperationRequestHandler<EmptyParameters, EmptyParameters>
    {
        private readonly Action<int> onAuthenticated;

        public AuthenticationOperationHandler(Action<int> onAuthenticated)
        {
            this.onAuthenticated = onAuthenticated;
        }

        public EmptyParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var dbUserId = 1; // TODO: Remove
            onAuthenticated.Invoke(dbUserId);
            return new EmptyParameters();
        }
    }
}