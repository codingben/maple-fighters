using System;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(CharacterDataProvider))]
    public class SpawnCharacter : MonoBehaviour, ISpawnedCharacter
    {
        public event Action CharacterSpawned;

        private GameObject spawnedCharacter;

        private ICharacterDataProvider characterDataProvider;
        private ICharacterSpriteGameObject characterSpriteGameObject;

        private void Awake()
        {
            characterDataProvider = GetComponent<ICharacterDataProvider>();
        }

        public void Spawn()
        {
            var characterData = characterDataProvider.GetCharacterData();
            var @class = characterData.Class;

            CreateCharacter((CharacterClasses)@class);
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

        private void CreateCharacter(CharacterClasses characterClass)
        {
            // Loading the character
            var path =
                string.Format(Paths.Resources.Game.Characters, characterClass);
            var characterObject = Resources.Load<GameObject>(path);
            var position = characterObject.transform.localPosition;

            // Creating the character
            spawnedCharacter = Instantiate(characterObject, transform);
            spawnedCharacter.transform.localPosition = position;
            spawnedCharacter.transform.SetAsFirstSibling();

            // Invoke
            CharacterSpawned?.Invoke();
        }
    }
}