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
            gameApi.GameObjectsAdded += OnGameObjectsAdded;
        }

        private void OnDisable()
        {
            gameApi.SceneEntered -= OnSceneEntered;
            gameApi.GameObjectsAdded -= OnGameObjectsAdded;
        }

        private void OnSceneEntered(EnteredSceneMessage message)
        {
            var id = message.GameObjectId;
            var characterName = message.CharacterData.Name;
            var characterClass = message.CharacterData.Class;
            var characterData = new CharacterData(id, characterName, characterClass);

            StartCoroutine(WaitFrameAndSpawn(characterData));
        }

        private void OnGameObjectsAdded(GameObjectsAddedMessage message)
        {
            foreach (var gameObject in message.GameObjects)
            {
                var id = gameObject.Id;
                var characterName = gameObject.CharacterData.Name;
                var characterClass = gameObject.CharacterData.Class;
                var characterData = new CharacterData(id, characterName, characterClass);

                StartCoroutine(WaitFrameAndSpawn(characterData));
            }
        }

        // TODO: Hack
        private IEnumerator WaitFrameAndSpawn(CharacterData characterData)
        {
            yield return null;

            var id = characterData.Id;

            if (EntityContainer.GetInstance().GetRemoteEntity(id, out var entity))
            {
                var characterDataProvider =
                    entity?.GameObject.GetComponent<CharacterDataProvider>();
                characterDataProvider?.SetCharacterData(characterData);

                var spawnedCharacter = entity?.GameObject.GetComponent<SpawnCharacter>();
                spawnedCharacter?.Spawn();
            }
        }
    }
}