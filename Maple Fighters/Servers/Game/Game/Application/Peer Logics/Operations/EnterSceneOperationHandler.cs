using Character.Client.Common;
using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.InterestManagement;
using ServerCommunicationHelper;
using Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterSceneOperationHandler : IOperationRequestHandler<EmptyParameters, EnterSceneResponseParameters>
    {
        private readonly ISceneObject sceneObject;
        private readonly CharacterFromDatabaseParameters character;

        public EnterSceneOperationHandler(ISceneObject sceneObject, CharacterFromDatabaseParameters character)
        {
            this.sceneObject = sceneObject;
            this.character = character;
        }

        public EnterSceneResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
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
            return new CharacterSpawnDetailsParameters(sceneObject.Id, character, transform.Direction.GetDirectionsFromDirection());
        }
    }
}