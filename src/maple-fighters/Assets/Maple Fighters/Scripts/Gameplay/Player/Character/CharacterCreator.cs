using System.Collections;
using Game.Common;
using Scripts.Gameplay.GameEntity;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.PlayerCharacter
{
    public class CharacterCreator : MonoBehaviour
    {
        private void Awake()
        {
            var gameService = GameService.GetInstance();
            gameService?.GameSceneApi.SceneEntered.AddListener(OnSceneEntered);
            gameService?.GameSceneApi.CharacterAdded.AddListener(OnCharacterAdded);
            gameService?.GameSceneApi.CharactersAdded.AddListener(OnCharactersAdded);
        }

        private void OnDestroy()
        {
            var gameService = GameService.GetInstance();
            gameService?.GameSceneApi.SceneEntered.RemoveListener(OnSceneEntered);
            gameService?.GameSceneApi.CharacterAdded.RemoveListener(OnCharacterAdded);
            gameService?.GameSceneApi.CharactersAdded.RemoveListener(OnCharactersAdded);
        }

        private void OnSceneEntered(EnterSceneResponseParameters parameters)
        {
            var characterSpawnDetails = parameters.Character;
            StartCoroutine(WaitFrameAndSpawn(characterSpawnDetails));
        }

        private void OnCharacterAdded(CharacterAddedEventParameters parameters)
        {
            var characterSpawnDetails = parameters.CharacterSpawnDetails;
            StartCoroutine(WaitFrameAndSpawn(characterSpawnDetails));
        }

        private void OnCharactersAdded(CharactersAddedEventParameters parameters)
        {
            var characterSpawnDetails = parameters.CharactersSpawnDetails;
            foreach (var characterSpawn in characterSpawnDetails)
            {
                StartCoroutine(WaitFrameAndSpawn(characterSpawn));
            }
        }

        // TODO: Hack
        private IEnumerator WaitFrameAndSpawn(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            yield return null;

            var id = characterSpawnDetails.SceneObjectId;
            var entity = EntityContainer.GetInstance().GetRemoteEntity(id)
                ?.GameObject;
            var spawnedCharacterDetails =
                entity?.GetComponent<SpawnedCharacterDetails>();
            spawnedCharacterDetails?.SetCharacterDetails(characterSpawnDetails);

            var spawnedCharacter = entity?.GetComponent<SpawnCharacter>();
            spawnedCharacter?.Spawn();
        }
    }
}