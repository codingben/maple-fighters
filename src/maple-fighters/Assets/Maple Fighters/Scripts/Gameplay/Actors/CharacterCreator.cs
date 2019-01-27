using Game.Common;
using Scripts.Containers;
using Scripts.Services;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreator : MonoBehaviour
    {
        private void Awake()
        {
            SubscribeToEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void SubscribeToEvents()
        {
            var gameScenePeerLogic = ServiceContainer.GameService
                .GetPeerLogic<IGameScenePeerLogicAPI>();
            gameScenePeerLogic.SceneEntered.AddListener(OnSceneEntered);
            gameScenePeerLogic.CharacterAdded.AddListener(OnCharacterAdded);
            gameScenePeerLogic.CharactersAdded.AddListener(OnCharactersAdded);
        }

        private void UnsubscribeFromEvents()
        {
            var gameScenePeerLogic = ServiceContainer.GameService
                .GetPeerLogic<IGameScenePeerLogicAPI>();
            gameScenePeerLogic.SceneEntered.RemoveListener(OnSceneEntered);
            gameScenePeerLogic.CharacterAdded.RemoveListener(OnCharacterAdded);
            gameScenePeerLogic.CharactersAdded.RemoveListener(OnCharactersAdded);
        }

        private void OnSceneEntered(
            EnterSceneResponseParameters parameters)
        {
            CreateCharacter(parameters.Character);
        }

        private void OnCharacterAdded(
            CharacterAddedEventParameters parameters)
        {
            CreateCharacter(parameters.CharacterSpawnDetails);
        }

        private void OnCharactersAdded(
            CharactersAddedEventParameters parameters)
        {
            foreach (var characterSpawnDetails in parameters
                .CharactersSpawnDetails)
            {
                CreateCharacter(characterSpawnDetails);
            }
        }

        private void CreateCharacter(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            var sceneObject = SceneObjectsContainer.GetInstance()
                .GetRemoteSceneObject(characterSpawnDetails.SceneObjectId);
            if (sceneObject != null)
            {
                var characterCreator = sceneObject.GameObject
                    .GetComponent<ICharacterCreator>();
                characterCreator?.Create(characterSpawnDetails);
            }
        }
    }
}