using System;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(SpawnedCharacterDetails))]
    public class SpawnCharacter : MonoBehaviour, ISpawnedCharacter
    {
        public event Action CharacterSpawned;

        private GameObject spawnedCharacter;
        private ISpawnedCharacterDetails spawnedCharacterDetails;

        private void Awake()
        {
            spawnedCharacterDetails = GetComponent<ISpawnedCharacterDetails>();
        }

        public void Spawn()
        {
            var characterDetails =
                spawnedCharacterDetails.GetCharacterDetails();
            var characterClass = characterDetails.Character.CharacterType;

            spawnedCharacter =
                SpawnCharacterUtils.Create(parent: transform, characterClass);

            CharacterSpawned?.Invoke();
        }

        public GameObject GetCharacterGameObject()
        {
            return spawnedCharacter;
        }

        public GameObject GetCharacterSpriteGameObject()
        {
            const int CharacterIndex = 0;

            var characterSprite =
                spawnedCharacter?.transform.GetChild(CharacterIndex);

            return characterSprite?.gameObject;
        }
    }
}