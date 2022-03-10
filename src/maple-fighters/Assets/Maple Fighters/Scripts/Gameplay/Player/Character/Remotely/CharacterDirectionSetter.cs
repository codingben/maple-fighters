using Scripts.Gameplay.Entity;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter), typeof(PositionSetter))]
    public class CharacterDirectionSetter : MonoBehaviour
    {
        private GameObject character;
        private PositionSetter positionSetter;

        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;

            positionSetter = GetComponent<PositionSetter>();
            positionSetter.PositionChanged += OnPositionChanged;
        }

        private void OnCharacterSpawned()
        {
            character = spawnedCharacter.GetCharacter();
        }

        private void OnPositionChanged(Vector3 position)
        {
            if (character == null)
            {
                return;
            }

            var transform = character.transform;
            var direction = (transform.position - position).normalized;
            var x = Mathf.Abs(transform.localScale.x);
            var y = transform.localScale.y;
            var z = transform.localScale.z;

            if (direction.x > 0)
            {
                transform.localScale = new Vector3(x, y, z);
            }
            else
            {
                transform.localScale = new Vector3(-x, y, z);
            }
        }
    }
}