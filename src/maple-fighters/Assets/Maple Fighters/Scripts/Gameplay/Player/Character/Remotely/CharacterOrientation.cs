using Game.Common;
using Scripts.Gameplay.Entity;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter), typeof(PositionSetter))]
    public class CharacterOrientation : MonoBehaviour
    {
        private Transform Character
        {
            get
            {
                if (character == null)
                {
                    character = transform;
                }

                return character;
            }

            set => character = value;
        }

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
            var characterGameObject = spawnedCharacter.GetCharacterGameObject();
            if (characterGameObject != null)
            {
                Character = characterGameObject.transform;
            }
        }

        private void OnDirectionChanged(Directions direction)
        {
            SetDirection(direction);
        }

        private void SetDirection(Directions direction)
        {
            var x = Mathf.Abs(Character.localScale.x);

            switch (direction)
            {
                case Directions.Left:
                {
                    Character.localScale = new Vector3(
                        x,
                        Character.localScale.y,
                        Character.localScale.z);
                    break;
                }

                case Directions.Right:
                {
                    Character.localScale = new Vector3(
                        -x,
                        Character.localScale.y,
                        Character.localScale.z);
                    break;
                }
            }
        }

        public Directions GetDirection()
        {
            var x = Character.localScale.x;
            var direction = x > 0 ? Directions.Left : Directions.Right;

            return direction;
        }
    }
}