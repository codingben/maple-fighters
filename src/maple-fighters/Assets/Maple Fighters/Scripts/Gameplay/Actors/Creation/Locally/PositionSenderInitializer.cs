using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnCharacter), typeof(PositionSender))]
    public class PositionSenderInitializer : MonoBehaviour
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
            var character = spawnedCharacter.GetCharacterGameObject();
            var positionSender = GetComponent<PositionSender>();
            positionSender.SetCharacter(character.transform);
        }
    }
}