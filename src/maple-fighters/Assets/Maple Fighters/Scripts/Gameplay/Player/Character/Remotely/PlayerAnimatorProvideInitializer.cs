using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class PlayerAnimatorProvideInitializer : MonoBehaviour
    {
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var animator = spawnedCharacter
                .GetCharacterSprite()
                .GetComponent<Animator>();
            if (animator != null)
            {
                var playerAnimatorProvider =
                    GetComponent<PlayerAnimatorProvider>();

                playerAnimatorProvider?.Initialize(animator);
            }
        }
    }
}