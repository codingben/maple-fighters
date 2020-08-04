using System.Collections;
using Game.Common;
using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services.Game;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class CharacterCreator : MonoBehaviour
    {
        private IGameApi gameApi;

        private void Start()
        {
            gameApi = FindObjectOfType<GameApi>();
            gameApi.SceneEntered += OnSceneEntered;
            gameApi.SceneObjectsAdded += OnSceneObjectsAdded;
        }

        private void OnDisable()
        {
            gameApi.SceneEntered -= OnSceneEntered;
            gameApi.SceneObjectsAdded -= OnSceneObjectsAdded;
        }

        private void OnSceneEntered(EnteredSceneMessage _)
        {
            // TODO: Get locally character data
            // StartCoroutine(WaitFrameAndSpawn(characterSpawnDetails));
        }

        private void OnSceneObjectsAdded(GameObjectsAddedMessage message)
        {
            // TODO: Get character data from message

            /*foreach (var characterSpawn in characterSpawnDetails)
            {
                StartCoroutine(WaitFrameAndSpawn(characterSpawn));
            }*/
        }

        // TODO: Hack
        private IEnumerator WaitFrameAndSpawn(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            yield return null;

            var id = characterSpawnDetails.SceneObjectId;

            if (EntityContainer.GetInstance().GetRemoteEntity(id, out var entity))
            {
                var spawnedCharacterDetails =
                    entity?.GameObject.GetComponent<SpawnedCharacterDetails>();
                spawnedCharacterDetails?.SetCharacterDetails(characterSpawnDetails);

                var spawnedCharacter = entity?.GameObject.GetComponent<SpawnCharacter>();
                spawnedCharacter?.Spawn();
            }
        }
    }
}