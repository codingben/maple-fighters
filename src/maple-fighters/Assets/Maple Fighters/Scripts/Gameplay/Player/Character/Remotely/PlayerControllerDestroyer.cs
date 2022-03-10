using Scripts.Gameplay.Map.Climb;
using Scripts.Gameplay.Map.Objects;
using Scripts.Gameplay.Player.Behaviours;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter))]
    public class PlayerControllerDestroyer : MonoBehaviour
    {
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var character = spawnedCharacter
                .GetCharacter();
            if (character != null)
            {
                Destroy(character.GetComponent<PlayerAttackedBehaviour>());
                Destroy(character.GetComponent<RopeInteractor>());
                Destroy(character.GetComponent<LadderInteractor>());
                Destroy(character.GetComponent<PortalInteractor>());
                Destroy(character.GetComponent<PlayerController>());
            }
        }
    }
}