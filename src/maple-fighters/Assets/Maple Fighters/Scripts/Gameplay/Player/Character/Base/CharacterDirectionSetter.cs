using UnityEngine;

namespace Scripts.Gameplay.Player
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
                var characterDetails =
                    spawnedCharacterDetails.GetCharacterDetails();
                var direction = characterDetails.Direction;
                var transform = 
                    spawnedCharacter.GetCharacterGameObject().transform;

                Utils.SetLocalScaleByDirection(ref transform, direction);
            }
        }
    }
}