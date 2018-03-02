using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.SceneObjects;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class ChangeSceneOperationHandler : IOperationRequestHandler<ChangeSceneRequestParameters, ChangeSceneResponseParameters>
    {
        private readonly ISceneObject sceneObject;
        private readonly ISceneContainer sceneContainer;
        private readonly ICharacterSpawnDetailsProvider CharacterSpawnDetailsProvider;
        private readonly ICharacterCreator characterCreator;

        public ChangeSceneOperationHandler(ISceneObject sceneObject)
        {
            this.sceneObject = sceneObject;

            sceneContainer = Server.Components.GetComponent<ISceneContainer>().AssertNotNull();
            CharacterSpawnDetailsProvider = Server.Components.GetComponent<ICharacterSpawnDetailsProvider>().AssertNotNull();
            characterCreator = Server.Components.GetComponent<ICharacterCreator>().AssertNotNull();
        }

        public ChangeSceneResponseParameters? Handle(MessageData<ChangeSceneRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            // Getting a current scene before the teleportation.
            var presenceSceneProvider = sceneObject.Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();

            // Getting portal info.
            var portalSceneObjectId = messageData.Parameters.PortalId;
            var portalSceneObject = presenceSceneProvider.Scene.GetSceneObject(portalSceneObjectId).AssertNotNull();
            var portalInfoProvider = portalSceneObject.Components.GetComponent<IPortalInfoProvider>().AssertNotNull();

            // Removing a body from old physics world.
            sceneObject.Components.RemoveComponent<CharacterBody>();

            // Removing a character from his old scene.
            presenceSceneProvider.Scene.RemoveSceneObject(sceneObject.Id);

            // Setting the character's position in the destination scene.
            var spawnPositionDetails = CharacterSpawnDetailsProvider.GetCharacterSpawnDetails(portalInfoProvider.Map);
            var transform = sceneObject.Components.GetComponent<ITransform>().AssertNotNull();
            transform.Position = spawnPositionDetails.Position;
            transform.Direction = spawnPositionDetails.Direction;

            // Adding a character to the destination scene.
            var destinationScene = sceneContainer.GetSceneWrapper(portalInfoProvider.Map).AssertNotNull();
            destinationScene.GetScene().AddSceneObject(sceneObject);

            // Setting the character's interest area in the destination scene.
            var interestArea = sceneObject.Components.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.SetSize();

            // Creating a new body in the new physics world.
            characterCreator.CreateCharacterBody(destinationScene, sceneObject);

            // Adding a new body to the new physics world.
            sceneObject.Components.AddComponent(new CharacterBody());
            return new ChangeSceneResponseParameters(portalInfoProvider.Map);
        }
    }
}