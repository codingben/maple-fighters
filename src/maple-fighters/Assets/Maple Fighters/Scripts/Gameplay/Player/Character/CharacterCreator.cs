using System.Collections;
using Game.Common;
using Scripts.Gameplay.GameEntity;
using Scripts.Network.Services;
using UnityEngine;

namespace Scripts.Gameplay.PlayerCharacter
{
    public class CharacterCreator : MonoBehaviour
    {
        private void Awake()
        {
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.SceneEntered.AddListener(OnSceneEntered);
            gameSceneApi?.CharacterAdded.AddListener(OnCharacterAdded);
            gameSceneApi?.CharactersAdded.AddListener(OnCharactersAdded);
        }

        private void OnDestroy()
        {
            var gameSceneApi = ServiceProvider.GameService.GetGameSceneApi();
            gameSceneApi?.SceneEntered.RemoveListener(OnSceneEntered);
            gameSceneApi?.CharacterAdded.RemoveListener(OnCharacterAdded);
            gameSceneApi?.CharactersAdded.RemoveListener(OnCharactersAdded);
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
            if (entity != null)
            {
                var spawnedCharacterDetails =
                    entity.GetComponent<SpawnedCharacterDetails>();
                spawnedCharacterDetails.SetCharacterDetails(characterSpawnDetails);

                var spawnedCharacter = entity.GetComponent<SpawnCharacter>();
                spawnedCharacter.Spawn();
            }
        }
    }
}