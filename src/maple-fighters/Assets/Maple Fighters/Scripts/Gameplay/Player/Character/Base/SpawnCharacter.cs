using System;
using Game.Common;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnedCharacterDetails))]
    public class SpawnCharacter : MonoBehaviour, ISpawnedCharacter
    {
        public event Action CharacterSpawned;

        private GameObject spawnedCharacter;

        private ISpawnedCharacterDetails spawnedCharacterDetails;
        private ICharacterSpriteGameObject characterSpriteGameObject;

        private void Awake()
        {
            spawnedCharacterDetails = GetComponent<ISpawnedCharacterDetails>();
        }

        public void Spawn()
        {
            var details = spawnedCharacterDetails.GetCharacterDetails();
            var type = details.Character.CharacterType;

            spawnedCharacter = Create(type);

            CharacterSpawned?.Invoke();
        }

        public GameObject GetCharacterGameObject()
        {
            return spawnedCharacter;
        }

        public GameObject GetCharacterSpriteGameObject()
        {
            if (characterSpriteGameObject == null)
            {
                characterSpriteGameObject = GetCharacterGameObject()
                    ?.GetComponent<ICharacterSpriteGameObject>();
            }

            return characterSpriteGameObject?.Provide();
        }

        private GameObject Create(CharacterClasses characterClass)
        {
            // Loading the character
            var path =
                string.Format(Paths.Resources.GameObjectsPath, characterClass);
            var characterObject = Resources.Load<GameObject>(path);
            var position = characterObject.transform.localPosition;

            // Creating the character
            var characterGameObject = Instantiate(characterObject, transform);
            characterGameObject.transform.localPosition = position;
            characterGameObject.transform.SetAsFirstSibling();

            return characterGameObject;
        }
    }
}