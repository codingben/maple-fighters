using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using MathematicsHelper;
using Physics.Box2D;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.Components
{
    internal class CharacterCreator : Component, ICharacterCreator
    {
        private const string SCENE_OBJECT_NAME = "Player";

        private ISceneContainer sceneContainer;
        private ICharacterSpawnPositionDetailsProvider characterSpawnPositionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneContainer = Entity.GetComponent<ISceneContainer>().AssertNotNull();
            characterSpawnPositionProvider = Server.Entity.GetComponent<ICharacterSpawnPositionDetailsProvider>().AssertNotNull();
        }

        public ISceneObject Create(Character character)
        {
            const Maps MAP = Maps.Map_1;

            var scene = sceneContainer.GetSceneWrapper(MAP).AssertNotNull();
            var spawnPositionDetails = characterSpawnPositionProvider.GetSpawnPositionDetails(MAP);
            var sceneObject = scene.GetScene().AddSceneObject(
                new SceneObject(SCENE_OBJECT_NAME, spawnPositionDetails.Position, spawnPositionDetails.Direction.FromDirections())).AssertNotNull();
            sceneObject.Container.AddComponent(new InterestArea(spawnPositionDetails.Position, scene.GetScene().RegionSize));
            sceneObject.Container.AddComponent(new InterestAreaNotifier());
            sceneObject.Container.AddComponent(new CharacterInformationProvider(character));
            sceneObject.Container.AddComponent(new CharacterBody());

            CreateCharacterBody(scene, sceneObject);
            return sceneObject;
        }

        public void CreateCharacterBody(IGameSceneWrapper sceneWrapper, ISceneObject sceneObject)
        {
            var spawnPosition = sceneObject.Container.GetComponent<ITransform>().AssertNotNull();
            var bodySize = new Vector2(0.3624894f, 0.825f); // TODO: Do not hard code it
            var bodyFixtureDefinition = PhysicsUtils.CreateFixtureDefinition(bodySize, LayerMask.Player);
            var bodyDefinition = PhysicsUtils.CreateBodyDefinitionWrapper(bodyFixtureDefinition, spawnPosition.InitialPosition, sceneObject);
            bodyDefinition.BodyDef.AllowSleep = false;

            var entityManager = sceneWrapper.GetScene().Entity.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.AddBody(new BodyInfo(sceneObject.Id, bodyDefinition));
        }
    }
}