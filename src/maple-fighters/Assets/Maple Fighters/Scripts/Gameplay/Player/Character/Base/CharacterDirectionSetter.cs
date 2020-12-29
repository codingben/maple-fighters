using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter), typeof(CharacterDataProvider))]
    public class CharacterDirectionSetter : MonoBehaviour
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
                // TODO: Use character data
                var characterData = characterDataProvider.GetCharacterData();

                // TODO: Change value
                var direction = Direction.Left;
                var transform =
                    spawnedCharacter.GetCharacterGameObject().transform;

                var x = Mathf.Abs(transform.localScale.x);
                var y = transform.localScale.y;
                var z = transform.localScale.z;

                if (direction == Direction.Left)
                {
                    transform.localScale = new Vector3(x, y, z);
                }
                else if (direction == Direction.Right)
                {
                    transform.localScale = new Vector3(-x, y, z);
                }
            }
        }
    }
}