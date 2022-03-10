using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class CharacterCollisionDisabler : MonoBehaviour
    {
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var collider = spawnedCharacter
                .GetCharacter()
                .GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.isTrigger = true;
            }
        }
    }
}