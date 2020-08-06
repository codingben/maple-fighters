using Scripts.Gameplay.Entity;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter), typeof(PositionSetter))]
    public class CharacterOrientationSetter : MonoBehaviour
    {
        private Transform character;
        private PositionSetter positionSetter;

        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            positionSetter = GetComponent<PositionSetter>();
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
        }

        private void Start()
        {
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
            positionSetter.PositionChanged += OnPositionChanged;
        }

        private void OnDestroy()
        {
            spawnedCharacter.CharacterSpawned -= OnCharacterSpawned;
            positionSetter.PositionChanged -= OnPositionChanged;
        }

        private void OnCharacterSpawned()
        {
            var characterGameObject =
                spawnedCharacter.GetCharacterGameObject();
            character = characterGameObject.transform;
        }

        private void OnPositionChanged(Vector3 position)
        {
            var direction = (character.position - position).normalized;
            var x = Mathf.Abs(character.localScale.x);
            var y = character.localScale.y;
            var z = character.localScale.z;

            if (direction.x < 0)
            {
                character.localScale = new Vector3(x, y, z);
            }
            else
            {
                character.localScale = new Vector3(-x, y, z);
            }
        }
    }
}