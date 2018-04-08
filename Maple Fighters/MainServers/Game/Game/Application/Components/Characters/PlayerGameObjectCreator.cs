using CommonTools.Log;
using ComponentModel.Common;
using Game.Application.Components.Interfaces;
using Game.Application.GameObjects;
using Game.Application.GameObjects.Components;
using Game.Common;
using InterestManagement.Components;

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
            var playerGameObject = CreatePlayerGameObject();
            playerGameObject.Components.AddComponent(new CharacterParametersGetter(character));
            return playerGameObject;
        }

        private PlayerGameObject CreatePlayerGameObject()
        {
            const Maps MAP = Maps.Map_1;

            var spawnDetails = characterSpawnDetailsProvider.GetCharacterSpawnDetails(MAP);
            var scene = sceneContainer.GetSceneWrapper(MAP).AssertNotNull($"Could not find a scene with map {MAP}").GetScene();
            var character = new PlayerGameObject(spawnDetails);
            var sceneObject = scene.AddSceneObject(character);
            sceneObject.Components.AddComponent(new InterestArea(spawnDetails.Position, scene.RegionSize));
            sceneObject.Components.AddComponent(new PlayerPositionController());
            return character;
        }
    }
}