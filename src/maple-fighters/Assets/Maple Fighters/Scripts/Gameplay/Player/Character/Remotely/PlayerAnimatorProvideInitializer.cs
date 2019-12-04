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
            var animator = spawnedCharacter
                .GetCharacterSpriteGameObject()
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