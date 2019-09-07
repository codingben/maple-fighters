using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class CharacterOrientation : MonoBehaviour, ICharacterOrientation
    {
        private Transform character;

        private void Awake()
        {
            var spawnedCharacter = GetComponent<ISpawnedCharacter>();
            if (spawnedCharacter != null)
            {
                character =
                    spawnedCharacter.GetCharacterGameObject().transform;
            }
            else
            {
                character = transform;
            }
        }

        public void SetDirection(Directions direction)
        {
            var x = Mathf.Abs(character.localScale.x);

            switch (direction)
            {
                case Directions.Left:
                {
                    character.localScale = new Vector3(
                        x,
                        character.localScale.y,
                        character.localScale.z);
                    break;
                }

                case Directions.Right:
                {
                    character.localScale = new Vector3(
                        -x,
                        character.localScale.y,
                        character.localScale.z);
                    break;
                }
            }
        }

        public Directions GetDirection()
        {
            var direction = Directions.None;

            if (character.localScale.x > 0)
            {
                direction = Directions.Left;
            }

            if (character.localScale.x < 0)
            {
                direction = Directions.Right;
            }

            return direction;
        }
    }
}