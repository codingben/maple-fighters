using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.PlayerCharacter
{
    [RequireComponent(typeof(SpawnCharacter), typeof(SpawnedCharacterDetails))]
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
            var spawnedCharacterDetails = GetComponent<ISpawnedCharacterDetails>();
            if (spawnedCharacterDetails != null)
            {
                var characterDetails = spawnedCharacterDetails.GetCharacterDetails();
                var direction = characterDetails.Direction;

                const float Scale = 1;

                var transform = 
                    spawnedCharacter.GetCharacterGameObject().transform;

                switch (direction)
                {
                    case Directions.Left:
                    {
                        transform.localScale = new Vector3(
                            Scale,
                            transform.localScale.y,
                            transform.localScale.z);
                        break;
                    }

                    case Directions.Right:
                    {
                        transform.localScale = new Vector3(
                            -Scale,
                            transform.localScale.y,
                            transform.localScale.z);
                        break;
                    }
                }
            }
        }
    }
}