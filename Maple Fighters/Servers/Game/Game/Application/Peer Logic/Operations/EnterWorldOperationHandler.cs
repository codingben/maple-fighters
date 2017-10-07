using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperationHandler : IOperationRequestHandler<EmptyParameters, EnterWorldOperationResponseParameters>
    {
        private readonly IGameObject gameObject;

        public EnterWorldOperationHandler(IGameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public EnterWorldOperationResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var transform = gameObject.Container.GetComponent<Transform>().AssertNotNull();
            var playerGameObject = new Shared.Game.Common.GameObject(gameObject.Id, "Local Player", transform.Position.X, transform.Position.Y);
            return new EnterWorldOperationResponseParameters(playerGameObject, new Character()); // TODO: Implement character
        }
    }
}