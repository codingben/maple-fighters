using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter), typeof(CharacterDataProvider))]
    public class CharacterSpawnDirectionSetter : MonoBehaviour
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
            var characterDataProvider = GetComponent<ICharacterDataProvider>();
            if (characterDataProvider != null)
            {
                var characterData = characterDataProvider.GetCharacterData();
                var direction = characterData.Direction;
                var transform = spawnedCharacter.GetCharacter().transform;
                var x = Mathf.Abs(transform.localScale.x);
                var y = transform.localScale.y;
                var z = transform.localScale.z;

                if (direction == 0)
                {
                    direction = 1;
                }

                transform.localScale = new Vector3(direction, y, z);
            }
        }
    }
}