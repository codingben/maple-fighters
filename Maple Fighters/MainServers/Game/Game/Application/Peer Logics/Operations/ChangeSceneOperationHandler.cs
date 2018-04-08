using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components.Interfaces;
using Game.Application.GameObjects;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Game.Common;
using InterestManagement.Components.Interfaces;

namespace Game.Application.PeerLogic.Operations
{
    internal class ChangeSceneOperationHandler : IOperationRequestHandler<ChangeSceneRequestParameters, ChangeSceneResponseParameters>
    {
        private readonly PlayerGameObject playerGameObject;
        private readonly ISceneContainer sceneContainer;
        private readonly ICharacterSpawnDetailsProvider CharacterSpawnDetailsProvider;

        public ChangeSceneOperationHandler(PlayerGameObject playerGameObject)
        {
            this.playerGameObject = playerGameObject;

            sceneContainer = ServerComponents.GetComponent<ISceneContainer>().AssertNotNull();
            CharacterSpawnDetailsProvider = ServerComponents.GetComponent<ICharacterSpawnDetailsProvider>().AssertNotNull();
        }

        public ChangeSceneResponseParameters? Handle(MessageData<ChangeSceneRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            // Getting a current scene before the teleportation.
            var presenceSceneProvider = playerGameObject.Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();

            // Getting portal info.
            var portalSceneObjectId = messageData.Parameters.PortalId;
            var portalSceneObject = presenceSceneProvider.GetScene().GetSceneObject(portalSceneObjectId).AssertNotNull();
            var portalInfoProvider = portalSceneObject.Components.GetComponent<IPortalInfoProvider>().AssertNotNull();

            // Removing body from the old physics world.
            playerGameObject.RemoveBody();

            // Removing a character from his old scene.
            presenceSceneProvider.GetScene().RemoveSceneObject(playerGameObject.Id, onDestroy: false);

            // Setting the character's position in the destination scene.
            var spawnPositionDetails = CharacterSpawnDetailsProvider.GetCharacterSpawnDetails(portalInfoProvider.Map);

            var positionTransform = playerGameObject.Components.GetComponent<IPositionTransform>().AssertNotNull();
            positionTransform.Position = spawnPositionDetails.Position;

            var directionTransform = playerGameObject.Components.GetComponent<IDirectionTransform>().AssertNotNull();
            directionTransform.SetDirection(spawnPositionDetails.Direction);

            // Adding a character to the destination scene.
            var destinationScene = sceneContainer.GetSceneWrapper(portalInfoProvider.Map).AssertNotNull($"Could not find a scene with map {portalInfoProvider.Map}");
            destinationScene.GetScene().AddSceneObject(playerGameObject, onAwake: false);

            // Setting the character's interest area in the destination scene.
            var interestArea = playerGameObject.Components.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.SetSize();

            // Creating a new body in the new physics world.
            playerGameObject.CreateBody();

            return new ChangeSceneResponseParameters(portalInfoProvider.Map);
        }
    }
}