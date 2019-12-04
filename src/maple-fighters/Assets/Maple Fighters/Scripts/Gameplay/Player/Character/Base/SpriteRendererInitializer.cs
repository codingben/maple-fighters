using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class SpriteRendererInitializer : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderIndex;

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
            var spriteRenderer = spawnedCharacter
                .GetCharacterSpriteGameObject()
                .GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = sortingOrderIndex;
            }
        }
    }
}