using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.Application.GameObjects.Components.Interfaces;
using Game.Common;
using InterestManagement.Components.Interfaces;
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

            var positionTransform = sceneObject.Components.GetComponent<IPositionTransform>().AssertNotNull();
            return new SceneObjectParameters(sceneObject.Id, SCENE_OBJECT_NAME, positionTransform.Position.X, positionTransform.Position.Y);
        }

        private CharacterSpawnDetailsParameters GetCharacterSpawnDetailsShared()
        {
            var characterGetter = sceneObject.Components.GetComponent<ICharacterParametersGetter>().AssertNotNull();
            var directionTransform = sceneObject.Components.GetComponent<IDirectionTransform>().AssertNotNull();
            return new CharacterSpawnDetailsParameters(sceneObject.Id, characterGetter.GetCharacter(), directionTransform.Direction.GetDirectionsFromDirection());
        }
    }
}