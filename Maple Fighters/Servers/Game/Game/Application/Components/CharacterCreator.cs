﻿using CommonTools.Log;
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
    internal class CharacterCreator : Component<IServerEntity>, ICharacterCreator
    {
        private const string SCENE_OBJECT_NAME = "Player";

        private ISceneContainer sceneContainer;
        private ICharacterSpawnPositionDetailsProvider characterSpawnPositionProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneContainer = Entity.Container.GetComponent<ISceneContainer>().AssertNotNull();
            characterSpawnPositionProvider = Server.Entity.Container.GetComponent<ICharacterSpawnPositionDetailsProvider>().AssertNotNull();
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

            CreateCharacterBody(scene, sceneObject);
            return sceneObject;
        }

        public void CreateCharacterBody(IGameSceneWrapper sceneWrapper, ISceneObject sceneObject)
        {
            var world = sceneWrapper.GetScene().Container.GetComponent<IPhysicsWorldProvider>().AssertNotNull().GetWorld();
            var spawnPosition = sceneObject.Container.GetComponent<ITransform>().AssertNotNull().InitialPosition;
            var characterBody = world.CreateCharacter(spawnPosition, new Vector2(0.3624894f, 1.070811f), LayerMask.Player); // TODO: Make a configurable size
            sceneObject.Container.AddComponent(new CharacterBody(characterBody, world));
            characterBody.SetUserData(sceneObject);
        }
    }
}