using System;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Game.Application.PeerLogic.Operations
{
    internal class AuthenticationOperationHandler : IOperationRequestHandler<EmptyParameters, EmptyParameters>
    {
        private readonly Action onAuthenticated;

        public AuthenticationOperationHandler(Action onAuthenticated)
        {
            this.onAuthenticated = onAuthenticated;
        }

        public EmptyParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            onAuthenticated.Invoke();
            return new EmptyParameters();
        }
    }
}