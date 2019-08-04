using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(CharacterInformationProvider))]
    public abstract class CharacterCreatorBase : MonoBehaviour
    {
        [Header("Sprite"), SerializeField]
        private int orderInLayer;

        private GameObject characterGameObject;
        private GameObject characterSpriteGameObject;

        private Directions direction;

        public virtual void Create(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            // Variables initialization
            direction = characterSpawnDetails.Direction;

            var characterName = characterSpawnDetails.Character.Name;
            var characterClass = characterSpawnDetails.Character.CharacterType;

            // The character creation
            characterGameObject = CreateCharacter(characterClass);

            // The character sprite game object
            characterSpriteGameObject = GetCharacterSpriteChild();

            // Calling other methods
            InitializeCharacterName(characterName);
            InitializeSpriteRenderer();
            InitializeCharacterInformationProvider(characterSpawnDetails.Character);

            ChangeCharacterDirection();
        }

        private GameObject CreateCharacter(CharacterClasses characterClass)
        {
            // The path
            const string GameObjectsPath = "Game/{0}";

            var gameObject =
                Resources.Load<GameObject>(
                    string.Format(GameObjectsPath, characterClass));

            // Creating the character
            var character =
                Instantiate(gameObject, Vector3.zero, Quaternion.identity, transform);

            // Sets the position
            character.transform.localPosition = gameObject.transform.localPosition;
            character.transform.SetAsFirstSibling();

            // The character name game object
            character.name =
                characterGameObject.name.RemoveCloneFromName();

            return character;
        }

        private GameObject GetCharacterSpriteChild()
        {
            const int CharacterIndex = 0;

            var transform =
                characterGameObject.transform.GetChild(CharacterIndex);

            return transform.gameObject;
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameSetter = characterSpriteGameObject
                .GetComponent<CharacterNameSetter>();
            if (characterNameSetter != null)
            {
                characterNameSetter.SetName(characterName);
                characterNameSetter.SetSortingOrder(orderInLayer);
            }
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer =
                characterSpriteGameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = orderInLayer;
            }
        }

        private void InitializeCharacterInformationProvider(
            CharacterParameters character)
        {
            var characterInformationProvider =
                GetComponent<CharacterInformationProvider>();
            if (characterInformationProvider != null)
            {
                characterInformationProvider.SetCharacterInformation(character);
            }
        }

        private void ChangeCharacterDirection()
        {
            const float Scale = 1;

            var transform = characterGameObject.transform;

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

        public GameObject GetCharacterGameObject()
        {
            return characterGameObject;
        }

        protected GameObject GetCharacterSpriteGameObject()
        {
            return characterSpriteGameObject;
        }
    }
}