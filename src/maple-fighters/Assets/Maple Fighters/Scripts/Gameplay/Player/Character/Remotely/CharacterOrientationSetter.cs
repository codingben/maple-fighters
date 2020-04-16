using Game.Common;
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
            positionSetter.DirectionChanged += OnDirectionChanged;
        }

        private void OnDestroy()
        {
            spawnedCharacter.CharacterSpawned -= OnCharacterSpawned;
            positionSetter.DirectionChanged -= OnDirectionChanged;
        }

        private void OnCharacterSpawned()
        {
            var characterGameObject = 
                spawnedCharacter.GetCharacterGameObject();
            character = characterGameObject.transform;
        }

        private void OnDirectionChanged(Directions direction)
        {
            Utils.SetLocalScaleByDirection(ref character, direction);
        }

        public Directions GetDirection()
        {
            var x = character.localScale.x;
            var direction = x > 0 ? Directions.Left : Directions.Right;

            return direction;
        }
    }
}