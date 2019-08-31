using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(CharacterGameObject))]
    public class CharacterCollisionDisabler : MonoBehaviour
    {
        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var collider = characterGameObject.GetCharacterGameObject()
                .GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }
    }

    [RequireComponent(typeof(CharacterGameObject))]
    public class PlayerControllerDestroyer : MonoBehaviour
    {
        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var components = characterGameObject.GetCharacterGameObject()
                .GetComponents<MonoBehaviour>();
            foreach (var component in components)
            {
                Destroy(component);
            }
        }
    }

    [RequireComponent(typeof(CharacterGameObject))]
    public class PlayerAnimatorProvideInitializer : MonoBehaviour
    {
        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var animator = characterGameObject.GetCharacterSpriteGameObject()
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