using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using ServerCommunicationHelper;
using Shared.Game.Common;
using GameObject = Shared.Game.Common.GameObject;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperationHandler : IOperationRequestHandler<EmptyParameters, EnterWorldResponseParameters>
    {
        private readonly IGameObject characterGameObject;
        private readonly Character character;

        public EnterWorldOperationHandler(IGameObject characterGameObject, Character character)
        {
            this.characterGameObject = characterGameObject;
            this.character = character;
        }

        public EnterWorldResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            return new EnterWorldResponseParameters(GetSharedGameObject(characterGameObject), character);
        }

        private GameObject GetSharedGameObject(IGameObject gameObject)
        {
            const string GAME_OBJECT_NAME = "Local Player";

            var transform = gameObject.Container.GetComponent<Transform>().AssertNotNull();
            return new GameObject(gameObject.Id, GAME_OBJECT_NAME, transform.Position.X, transform.Position.Y);
        }
    }
}