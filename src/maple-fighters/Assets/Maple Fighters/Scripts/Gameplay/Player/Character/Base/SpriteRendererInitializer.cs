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
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var spriteRenderer = spawnedCharacter
                .GetCharacterSprite()
                .GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = sortingOrderIndex;
            }
        }
    }
}