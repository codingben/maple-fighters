using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.Components.Interfaces;
using Game.Application.SceneObjects.Components;
using ServerApplication.Common.ApplicationBase;
using ServerApplication.Common.Components;
using Game.Common;
using InterestManagement.Components;
using InterestManagement.Components.Interfaces;
using Physics.Box2D.Components.Interfaces;
using Physics.Box2D.Core;

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
            var sizeTransform = sceneObject.Components.GetComponent<ISizeTransform>().AssertNotNull();
            var positionTransform = sceneObject.Components.GetComponent<IPositionTransform>().AssertNotNull();

            var bodyFixtureDefinition = PhysicsUtils.CreateFixtureDefinition(sizeTransform.Size, LayerMask.Player);
            var bodyDefinition = PhysicsUtils.CreateBodyDefinitionWrapper(bodyFixtureDefinition, positionTransform.Position, sceneObject);
            bodyDefinition.BodyDefiniton.AllowSleep = false;

            var entityManager = sceneWrapper.GetScene().Components.GetComponent<IEntityManager>().AssertNotNull();
            entityManager.AddBody(new BodyInfo(sceneObject.Id, bodyDefinition));
        }
    }
}