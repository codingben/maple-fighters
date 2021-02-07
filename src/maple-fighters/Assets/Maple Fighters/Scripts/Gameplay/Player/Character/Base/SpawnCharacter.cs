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
                var character =
                    GetCharacterGameObject();
                characterSprite =
                    character?.GetComponent<ICharacterSpriteGameObject>();
            }

            return characterSprite.Provide();
        }

        private void CreateCharacter(CharacterClasses characterClass)
        {
            var path = Paths.Resources.Game.Characters;
            var characterPath = string.Format(path, characterClass);
            var characterObject = Resources.Load<GameObject>(path);
            if (characterObject != null)
            {
                spawnedCharacter = Instantiate(characterObject, transform);

                if (spawnedCharacter != null)
                {
                    var position = characterObject.transform.localPosition;

                    spawnedCharacter.transform.localPosition = position;
                    spawnedCharacter.transform.SetAsFirstSibling();

                    CharacterSpawned?.Invoke();
                }
            }
        }
    }
}