using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Components;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class ChangeSceneOperationHandler : IOperationRequestHandler<ChangeSceneRequestParameters, EmptyParameters>
    {
        private readonly CharacterSceneObjectGetter sceneObjectGetter;
        private readonly SceneContainer sceneContainer;

        public ChangeSceneOperationHandler(CharacterSceneObjectGetter sceneObjectGetter)
        {
            this.sceneObjectGetter = sceneObjectGetter;

            sceneContainer = Server.Entity.Container.GetComponent<SceneContainer>().AssertNotNull();
        }

        public EmptyParameters? Handle(MessageData<ChangeSceneRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var sceneId = messageData.Parameters.Map;
            var scene = sceneContainer.GetGameSceneWrapper(sceneId).AssertNotNull();

            var sceneObject = sceneObjectGetter.GetSceneObject();
            sceneObject.Scene.RemoveSceneObject(sceneObject.Id);

            scene.GetScene().AddSceneObject(sceneObject);

            return new EmptyParameters();
        }
    }
}