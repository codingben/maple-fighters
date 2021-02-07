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
        private ICharacterSpriteGameObject characterSprite;

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
            if (characterSprite == null)
            {
                characterSprite =
                    GetCharacterGameObject().GetComponent<ICharacterSpriteGameObject>();
            }

            return characterSprite.Provide();
        }

        private void CreateCharacter(CharacterClasses characterClass)
        {
            var path =
                string.Format(Paths.Resources.Game.Characters, characterClass);
            var character = Resources.Load<GameObject>(path);
            if (character != null)
            {
                var position = character.transform.localPosition;

                spawnedCharacter = Instantiate(character, transform);
                spawnedCharacter.transform.localPosition = position;
                spawnedCharacter.transform.SetAsFirstSibling();

                CharacterSpawned?.Invoke();
            }
        }
    }
}