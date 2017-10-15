using System;
using CommonCommunicationInterfaces;
using ServerCommunicationHelper;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperationHandler : IOperationRequestHandler<EmptyParameters, EmptyParameters>
    {
        private readonly Action onEnterWorld;

        public EnterWorldOperationHandler(Action onEnterWorld)
        {
            this.onEnterWorld = onEnterWorld;
        }

        public EmptyParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            onEnterWorld.Invoke();
            return null;
        }
    }
}