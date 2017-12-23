using CommonCommunicationInterfaces;
using CommonTools.Log;
using Game.Application.Components;
using Game.Application.PeerLogic.Components;
using Game.Application.SceneObjects;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using ServerApplication.Common.ApplicationBase;
using ServerCommunicationHelper;
using Shared.Game.Common;

namespace Game.Application.PeerLogic.Operations
{
    internal class ChangeSceneOperationHandler : IOperationRequestHandler<ChangeSceneRequestParameters, ChangeSceneResponseParameters>
    {
        private readonly ICharacterGetter character;
        private readonly ISceneContainer sceneContainer;
        private readonly ICharacterSpawnPositionDetailsProvider characterSpawnPositionProvider;
        private readonly ICharacterCreator characterCreator;

        public ChangeSceneOperationHandler(ICharacterGetter character)
        {
            this.character = character;

            sceneContainer = Server.Entity.Container.GetComponent<ISceneContainer>().AssertNotNull();
            characterSpawnPositionProvider = Server.Entity.Container.GetComponent<ICharacterSpawnPositionDetailsProvider>().AssertNotNull();
            characterCreator = Server.Entity.Container.GetComponent<ICharacterCreator>().AssertNotNull();
        }

        public ChangeSceneResponseParameters? Handle(MessageData<ChangeSceneRequestParameters> messageData, ref MessageSendOptions sendOptions)
        {
            var characterSceneObject = character.GetSceneObject();

            // Getting portal info.
            var portalSceneObjectId = messageData.Parameters.PortalId;
            var portalSceneObject = characterSceneObject.Scene.GetSceneObject(portalSceneObjectId).AssertNotNull();
            var portalInfoProvider = portalSceneObject.Container.GetComponent<IPortalInfoProvider>().AssertNotNull();

            // Removing a body from old physics world.
            characterSceneObject.Container.RemoveComponent<CharacterBody>();

            // Removing a character from his old scene.
            characterSceneObject.Scene.RemoveSceneObject(characterSceneObject.Id);

            // Setting the character's position in the destination scene.
            var newPlayerPosition = characterSpawnPositionProvider.GetSpawnPositionDetails(portalInfoProvider.Map);
            var transform = characterSceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            transform.InitialPosition = newPlayerPosition.Position;
            transform.Direction = newPlayerPosition.Direction;

            // Adding a character to the destination scene.
            var destinationScene = sceneContainer.GetSceneWrapper(portalInfoProvider.Map).AssertNotNull();
            destinationScene.GetScene().AddSceneObject(characterSceneObject);

            // Setting the character's interest area in the destination scene.
            var interestArea = characterSceneObject.Container.GetComponent<IInterestArea>().AssertNotNull();
            interestArea.SetSize();

            // Creating a new body in the new physics world.
            characterCreator.CreateCharacterBody(destinationScene, characterSceneObject);

            // Adding a new body to the new physics world.
            characterSceneObject.Container.AddComponent(new CharacterBody());

            return new ChangeSceneResponseParameters(portalInfoProvider.Map);
        }
    }
}