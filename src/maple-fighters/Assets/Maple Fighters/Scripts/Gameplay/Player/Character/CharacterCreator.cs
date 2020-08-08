using System.Collections;
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
        private IEnumerator WaitFrameAndSpawn(CharacterData characterData)
        {
            yield return null;

            var id = characterData.Id;

            if (EntityContainer.GetInstance().GetRemoteEntity(id, out var entity))
            {
                var spawnedCharacterDetails =
                    entity?.GameObject.GetComponent<SpawnedCharacterDetails>();
                spawnedCharacterDetails?.SetCharacterDetails(characterData);

                var spawnedCharacter = entity?.GameObject.GetComponent<SpawnCharacter>();
                spawnedCharacter?.Spawn();
            }
        }
    }
}