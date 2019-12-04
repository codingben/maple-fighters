using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class PlayerControllerInitializer : MonoBehaviour
    {
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
        }

        private void Start()
        {
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnDestroy()
        {
            spawnedCharacter.CharacterSpawned -= OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var playerStateAnimator = spawnedCharacter
                .GetCharacterSpriteGameObject()
                .AddComponent<PlayerStateAnimator>();
            if (playerStateAnimator != null)
            {
                var playerController = spawnedCharacter
                    .GetCharacterGameObject()
                    .GetComponent<PlayerController>();
                playerController?.SetPlayerStateAnimator(playerStateAnimator);
            }
        }
    }
}