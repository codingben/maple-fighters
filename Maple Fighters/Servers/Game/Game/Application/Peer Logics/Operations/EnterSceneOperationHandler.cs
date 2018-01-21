using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.PeerLogic.Components;
using Game.InterestManagement;
using ServerCommunicationHelper;
using Shared.Game.Common;
using SceneObject = Shared.Game.Common.SceneObject;

namespace Game.Application.PeerLogic.Operations
{
    internal class EnterSceneOperationHandler : IOperationRequestHandler<EmptyParameters, EnterSceneResponseParameters>
    {
        private readonly ISceneObject sceneObject;
        private readonly Character character;

        public EnterSceneOperationHandler(ISceneObject sceneObject, Character character)
        {
            this.sceneObject = sceneObject;
            this.character = character;
        }

        public EnterSceneResponseParameters? Handle(MessageData<EmptyParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var interestArea = sceneObject.Container.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.DetectOverlapsWithRegions();
            return new EnterSceneResponseParameters(GetSharedSceneObject(), GetSharedCharacterInformation());
        }

        private SceneObject GetSharedSceneObject()
        {
            const string SCENE_OBJECT_NAME = "Local Player";

            var transform = sceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            return new SceneObject(sceneObject.Id, SCENE_OBJECT_NAME, transform.Position.X, transform.Position.Y);
        }

        private CharacterInformation GetSharedCharacterInformation()
        {
            var orientationProvider = sceneObject.Container.GetComponent<IOrientationProvider>().AssertNotNull();
            return new CharacterInformation(sceneObject.Id, character.Name, character.CharacterType, orientationProvider.Direction.GetDirectionsFromDirection());
        }
    }
}