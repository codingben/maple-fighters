using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;
using GameObject = Shared.Game.Common.GameObject;

namespace Game.Application.PeerLogic.Components
{
    internal class LocalGameObjectFetcher : Component<IPeerEntity>
    {
        private EventSenderWrapper eventSender;
        private CharacterGameObjectGetter gameObjectGetter;

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();
            gameObjectGetter = Entity.Container.GetComponent<CharacterGameObjectGetter>().AssertNotNull();

            SendLocalGameObject();
        }

        private void SendLocalGameObject()
        {
            var characterGameObject = gameObjectGetter.GetGameObject();
            var character = gameObjectGetter.GetCharacter();
            var parameters = new LocalGameObjectAddedEventParameters(GetSharedGameObject(characterGameObject), character);
            eventSender.Send((byte)GameEvents.LocalGameObjectAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private GameObject GetSharedGameObject(IGameObject gameObject)
        {
            const string GAME_OBJECT_NAME = "Local Player";

            var transform = gameObject.Container.GetComponent<Transform>().AssertNotNull();
            return new GameObject(gameObject.Id, GAME_OBJECT_NAME, transform.Position.X, transform.Position.Y);
        }
    }
}