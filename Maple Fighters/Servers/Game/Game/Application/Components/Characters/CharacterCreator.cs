using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.SceneObjects.Components;
using Game.InterestManagement;
using Physics.Box2D;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;
using Game.Common;
using SceneObject = Game.InterestManagement.SceneObject;

namespace Game.Application.Components
{
    internal class CharacterCreator : Component, ICharacterCreator
    {
        private const string SCENE_OBJECT_NAME = "Player";

        private ISceneContainer sceneContainer;
        private ICharacterSpawnDetailsProvider characterSpawnDetailsProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneContainer = Components.GetComponent<ISceneContainer>().AssertNotNull();
            characterSpawnDetailsProvider = Server.Components.GetComponent<ICharacterSpawnDetailsProvider>().AssertNotNull();
        }

        public ISceneObject Create(CharacterParameters character)
        {
            const Maps MAP = Maps.Map_1;

            var scene = sceneContainer.GetSceneWrapper(MAP).AssertNotNull();
            var spawnDetails = characterSpawnDetailsProvider.GetCharacterSpawnDetails(MAP);
            var id = IdGenerator.GetId();
            var sceneObject = scene.GetScene().AddSceneObject(new SceneObject(id, SCENE_OBJECT_NAME, spawnDetails));
            sceneObject.Components.AddComponent(new InterestArea(spawnDetails.Position, scene.GetScene().RegionSize));
            sceneObject.Components.AddComponent(new InterestAreaNotifier());
            sceneObject.Components.AddComponent(new CharacterGetter(character));
            sceneObject.Components.AddComponent(new CharacterBody());

            CreateCharacterBody(scene, sceneObject);
            return sceneObject;
        }

        public void CreateCharacterBody(IGameSceneWrapper sceneWrapper, ISceneObject sceneObject)
        {
            var spawnDetails = sceneObject.Components.GetComponent<ITransform>().AssertNotNull();

            var bodyFixtureDefinition = PhysicsUtils.CreateFixtureDefinition(spawnDetails.Size, LayerMask.Player);
            var bodyDefinition = PhysicsUtils.CreateBodyDefinitionWrapper(bodyFixtureDefinition, spawnDetails.Position, sceneObject);
            bodyDefinition.BodyDefiniton.AllowSleep = false;

            var entityManager = sceneWrapper.GetScene().Components.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.AddBody(new BodyInfo(sceneObject.Id, bodyDefinition));
        }
    }
}