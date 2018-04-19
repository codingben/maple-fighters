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
            DestroyBodyFromOldWorld();

            // Removing a character from his old scene.
            RemoveSceneObject();

            // Setting the character's position in the destination scene.
            ChangeTransform();

            // Adding a character to the destination scene.
            AddSceneObject();

            // Setting the character's interest area in the destination scene.
            FixInterestAreaSize();

            // Creating a new body in the new physics world.
            CreateBodyToNewWorld();

            return new ChangeSceneResponseParameters(portalInfoProvider.Map);

            void DestroyBodyFromOldWorld()
            {
                playerGameObject.RemoveBody();
            }

            void RemoveSceneObject()
            {
                presenceSceneProvider.GetScene().RemoveSceneObject(playerGameObject);
                presenceSceneProvider.SetScene(null);
            }

            void ChangeTransform()
            {
                var spawnPositionDetails = CharacterSpawnDetailsProvider.GetCharacterSpawnDetails(portalInfoProvider.Map);

                var positionTransform = playerGameObject.Components.GetComponent<IPositionTransform>().AssertNotNull();
                positionTransform.Position = spawnPositionDetails.Position;

                var directionTransform = playerGameObject.Components.GetComponent<IDirectionTransform>().AssertNotNull();
                directionTransform.SetDirection(spawnPositionDetails.Direction);
            }

            void AddSceneObject()
            {
                var destinationScene = sceneContainer.GetSceneWrapper(portalInfoProvider.Map).AssertNotNull($"Could not find a scene with map {portalInfoProvider.Map}");
                destinationScene.GetScene().AddSceneObject(playerGameObject);

                // Setting a new scene.
                presenceSceneProvider.SetScene(destinationScene.GetScene());
            }

            void FixInterestAreaSize()
            {
                var interestArea = playerGameObject.Components.GetComponent<IInterestArea>().AssertNotNull();
                interestArea.SetSize();
            }

            void CreateBodyToNewWorld()
            {
                playerGameObject.CreateBody();
            }
        }
    }
}