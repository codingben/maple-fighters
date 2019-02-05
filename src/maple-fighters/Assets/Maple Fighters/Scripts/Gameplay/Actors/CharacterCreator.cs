using System.Collections;
using Game.Common;
using Scripts.Containers;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreator : MonoBehaviour
    {
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
            StartCoroutine(WaitFrameAndCreateCharacter(characterSpawnDetails));
        }

        // TODO: Hack
        private IEnumerator WaitFrameAndCreateCharacter(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            yield return null;

            var sceneObject =
                SceneObjectsContainer.GetInstance()
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