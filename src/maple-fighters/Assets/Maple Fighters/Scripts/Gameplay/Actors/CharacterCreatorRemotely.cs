using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class CharacterCollisionDisabler : MonoBehaviour
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
            var collider = spawnedCharacter
                .GetCharacterGameObject()
                .GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }
    }

    [RequireComponent(typeof(SpawnCharacter))]
    public class PlayerControllerDestroyer : MonoBehaviour
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
            var components = spawnedCharacter
                .GetCharacterGameObject()
                .GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }
    }

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
                playerAnimatorProvider.Initialize(animator);
            }
        }
    }
}