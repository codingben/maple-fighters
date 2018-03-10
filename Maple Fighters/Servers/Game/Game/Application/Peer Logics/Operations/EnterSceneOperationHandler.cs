using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.Application.SceneObjects.Components;
using Game.Common;
using Game.InterestManagement;
using ServerCommunicationHelper;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterSceneOperationHandler : IOperationRequestHandler<EmptyParameters, EnterSceneResponseParameters>
    {
        private readonly ISceneObject sceneObject;

        public EnterSceneOperationHandler(ISceneObject sceneObject)
        {
            this.sceneObject = sceneObject;
        }

        public EnterSceneResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            return EnterScene();
        }

        private EnterSceneResponseParameters EnterScene()
        {
            var interestArea = sceneObject.Components.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.DetectOverlapsWithRegions();
            return new EnterSceneResponseParameters(GetSceneObjectShared(), GetCharacterSpawnDetailsShared());
        }

        private SceneObjectParameters GetSceneObjectShared()
        {
            const string SCENE_OBJECT_NAME = "Local Player";

            var transform = sceneObject.Components.GetComponent<ITransform>().AssertNotNull();
            return new SceneObjectParameters(sceneObject.Id, SCENE_OBJECT_NAME, transform.Position.X, transform.Position.Y);
        }

        private CharacterSpawnDetailsParameters GetCharacterSpawnDetailsShared()
        {
            var transform = sceneObject.Components.GetComponent<ITransform>().AssertNotNull();
            var character = sceneObject.Components.GetComponent<ICharacterGetter>().AssertNotNull();
            return new CharacterSpawnDetailsParameters(sceneObject.Id, character.GetCharacter(), transform.Direction.GetDirectionsFromDirection());
        }
    }
}