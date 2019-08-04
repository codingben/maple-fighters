using System;
using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface ICharacterCreator
    {
        event Action<CharacterSpawnDetailsParameters> CreateCharacter;
    }

    public class CharacterCreator : MonoBehaviour, ICharacterCreator
    {
        public event Action<CharacterSpawnDetailsParameters> CreateCharacter;

        private void Awake()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.SceneEntered.AddListener(OnSceneEntered);
                gameSceneApi.CharacterAdded.AddListener(OnCharacterAdded);
                gameSceneApi.CharactersAdded.AddListener(OnCharactersAdded);
            }
        }
        
        private void OnDestroy()
        {
            var gameSceneApi = ServiceContainer.GameService.GetGameSceneApi();
            if (gameSceneApi != null)
            {
                gameSceneApi.SceneEntered.RemoveListener(OnSceneEntered);
                gameSceneApi.CharacterAdded.RemoveListener(OnCharacterAdded);
                gameSceneApi.CharactersAdded.RemoveListener(OnCharactersAdded);
            }
        }

        private void OnSceneEntered(
            EnterSceneResponseParameters parameters)
        {
            CreateCharacter?.Invoke(parameters.Character);
        }

        private void OnCharacterAdded(
            CharacterAddedEventParameters parameters)
        {
            CreateCharacter?.Invoke(parameters.CharacterSpawnDetails);
        }

        private void OnCharactersAdded(
            CharactersAddedEventParameters parameters)
        {
            foreach (var characterSpawnDetails in parameters
                .CharactersSpawnDetails)
            {
                CreateCharacter?.Invoke(characterSpawnDetails);
            }
        }
    }
}