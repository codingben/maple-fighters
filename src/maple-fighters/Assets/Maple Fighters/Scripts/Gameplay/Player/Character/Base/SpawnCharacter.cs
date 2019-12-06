using System;
using Game.Common;
using Scripts.Constants;
using UI.Manager;
using UnityEngine;

namespace Scripts.Gameplay.Player
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

            spawnedCharacter = Create(characterClass);

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

        private GameObject Create(CharacterClasses characterClass)
        {
            // Loading the character
            var path =
                string.Format(Paths.Resources.GameObjectsPath, characterClass);
            var characterObject = Resources.Load<GameObject>(path);

            // Creating the character
            var spawnedCharacter =
                Instantiate(characterObject, Vector3.zero, Quaternion.identity, transform);

            // Sets the position
            spawnedCharacter.transform.localPosition = characterObject.transform.localPosition;
            spawnedCharacter.transform.SetAsFirstSibling();

            // Sets the character name
            spawnedCharacter.name = spawnedCharacter.name.RemoveCloneFromName();

            return spawnedCharacter;
        }
    }
}