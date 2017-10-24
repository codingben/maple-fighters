using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.InterestManagement;
using ServerCommunicationHelper;
using Shared.Game.Common;
using SceneObject = Shared.Game.Common.SceneObject;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterWorldOperationHandler : IOperationRequestHandler<EmptyParameters, EnterWorldResponseParameters>
    {
        private readonly ICharacterGetter sceneObjectGetter;

        public EnterWorldOperationHandler(ICharacterGetter sceneObjectGetter)
        {
            this.sceneObjectGetter = sceneObjectGetter;
        }

        public EnterWorldResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterSceneObject = sceneObjectGetter.GetSceneObject();
            var character = sceneObjectGetter.GetCharacter();
            return new EnterWorldResponseParameters(GetSharedSceneObject(characterSceneObject), character);
        }

        private SceneObject GetSharedSceneObject(ISceneObject sceneObject)
        {
            const string SCENE_OBJECT_NAME = "Local Player";

            var transform = sceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            return new SceneObject(sceneObject.Id, SCENE_OBJECT_NAME, transform.InitialPosition.X, transform.InitialPosition.Y);
        }
    }
}