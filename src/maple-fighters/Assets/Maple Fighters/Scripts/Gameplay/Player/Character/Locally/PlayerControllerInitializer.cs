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
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var playerStateAnimator = spawnedCharacter
                .GetCharacterSprite()
                .GetComponent<PlayerStateAnimator>();
            if (playerStateAnimator != null)
            {
                var playerController = spawnedCharacter
                    .GetCharacter()
                    .GetComponent<PlayerController>();

                playerController?.SetPlayerStateAnimator(playerStateAnimator);
            }
        }
    }
}