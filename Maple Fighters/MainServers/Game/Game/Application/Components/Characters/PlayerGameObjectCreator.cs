using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.Components.Interfaces;
using Game.Application.GameObjects;
using Game.Application.GameObjects.Components;
using Game.Common;
using InterestManagement.Components;
using InterestManagement.Components.Interfaces;

namespace Game.Application.Components
{
    internal class PlayerGameObjectCreator : Component, IPlayerGameObjectCreator
    {
        private ISceneContainer sceneContainer;
        private ICharacterSpawnDetailsProvider characterSpawnDetailsProvider;

        protected override void OnAwake()
        {
            base.OnAwake();

            sceneContainer = Components.GetComponent<ISceneContainer>().AssertNotNull();
            characterSpawnDetailsProvider = Components.GetComponent<ICharacterSpawnDetailsProvider>().AssertNotNull();
        }

        public PlayerGameObject Create(CharacterParameters character)
        {
            var map = character.LastMap;
            var playerGameObject = CreatePlayerGameObject(map);
            playerGameObject.Components.AddComponent(new CharacterParametersGetter(character));
            return playerGameObject;
        }

        private PlayerGameObject CreatePlayerGameObject(Maps map)
        {
            var spawnDetails = characterSpawnDetailsProvider.GetCharacterSpawnDetails(map);
            var scene = sceneContainer.GetSceneWrapper(map).AssertNotNull($"Could not find a scene with map {map}").GetScene();
            var character = new PlayerGameObject(spawnDetails);
            var sceneObject = scene.AddSceneObject(character);
            var presenceSceneProvider = sceneObject.Components.GetComponent<IPresenceSceneProvider>().AssertNotNull();
            presenceSceneProvider.SetScene(scene);

            AddCommonComponents();

            sceneObject.Awake();
            return character;

            void AddCommonComponents()
            {
                sceneObject.Components.AddComponent(new InterestArea(spawnDetails.Position, scene.RegionSize));
                sceneObject.Components.AddComponent(new PlayerPositionController());
            }
        }
    }
}