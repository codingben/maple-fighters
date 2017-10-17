using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.InterestManagement;
using ServerApplication.Common.ComponentModel;
using ServerApplication.Common.Components;
using Shared.Game.Common;
using Shared.ServerApplication.Common.PeerLogic;
using SceneObject = Shared.Game.Common.SceneObject;

namespace Game.Application.PeerLogic.Components
{
    internal class LocalCharacterSender : Component<IPeerEntity>
    {
        private EventSenderWrapper eventSender;
        private CharacterSceneObjectGetter sceneObjectGetter;

        protected override void OnAwake()
        {
            base.OnAwake();

            eventSender = Entity.Container.GetComponent<EventSenderWrapper>().AssertNotNull();
            sceneObjectGetter = Entity.Container.GetComponent<CharacterSceneObjectGetter>().AssertNotNull();

            SendLocalSceneObject();
        }

        private void SendLocalSceneObject()
        {
            var characterSceneObject = sceneObjectGetter.GetSceneObject();
            var character = sceneObjectGetter.GetCharacter();
            var parameters = new LocalSceneObjectAddedEventParameters(GetSharedSceneObject(characterSceneObject), character);
            eventSender.Send((byte)GameEvents.LocalSceneObjectAdded, parameters, MessageSendOptions.DefaultReliable());
        }

        private SceneObject GetSharedSceneObject(ISceneObject sceneObject)
        {
            const string SCENE_OBJECT_NAME = "Local Player";

            var transform = sceneObject.Container.GetComponent<Transform>().AssertNotNull();
            return new SceneObject(sceneObject.Id, SCENE_OBJECT_NAME, transform.Position.X, transform.Position.Y);
        }
    }
}