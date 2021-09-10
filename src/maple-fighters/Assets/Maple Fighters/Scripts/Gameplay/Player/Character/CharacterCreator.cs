using System.Collections;
using Game.Messages;
using Scripts.Gameplay.Entity;
using Scripts.Services;
using Scripts.Services.GameApi;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class CharacterCreator : MonoBehaviour
    {
        private IGameApi gameApi;

        private void Start()
        {
            gameApi = ApiProvider.ProvideGameApi();
            gameApi.SceneEntered.AddListener(OnSceneEntered);
            gameApi.GameObjectsAdded.AddListener(OnGameObjectsAdded);
        }

        private void OnDisable()
        {
            gameApi.SceneEntered.RemoveListener(OnSceneEntered);
            gameApi.GameObjectsAdded.RemoveListener(OnGameObjectsAdded);
        }

        private void OnSceneEntered(EnteredSceneMessage message)
        {
            var id = message.GameObjectId;
            var name = message.CharacterName;
            var @class = message.CharacterClass;
            var direction = message.Direction;
            var characterData = new CharacterData(name, @class, direction);

            StartCoroutine(WaitFrameAndSpawn(id, characterData));
        }

        private void OnGameObjectsAdded(GameObjectsAddedMessage message)
        {
            foreach (var gameObject in message.GameObjects)
            {
                var id = gameObject.Id;
                var name = gameObject.CharacterName;
                var @class = gameObject.CharacterClass;
                var direction = gameObject.Direction;
                var characterData = new CharacterData(name, @class, direction);

                StartCoroutine(WaitFrameAndSpawn(id, characterData));
            }
        }

        // NOTE: Hack
        private IEnumerator WaitFrameAndSpawn(int entityId, CharacterData characterData)
        {
            yield return null;

            if (EntityContainer.GetInstance().GetRemoteEntity(entityId, out var entity))
            {
                var characterDataProvider =
                    entity?.GameObject?.GetComponent<CharacterDataProvider>();
                var spawnCharacter =
                    entity?.GameObject?.GetComponent<SpawnCharacter>();

                characterDataProvider?.SetCharacterData(characterData);
                spawnCharacter?.Spawn();
            }
        }
    }
}