using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(CharacterInformationProvider))]
    public abstract class CharacterCreatorBase : MonoBehaviour, ICharacterCreator
    {
        [Header("Sprite"), SerializeField]
        private int orderInLayer;

        private GameObject characterGameObject;
        private GameObject characterSpriteGameObject;

        private Directions direction;

        public virtual void Create(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            // The path
            const string GameObjectsPath = "Game/{0}";
            const int CharacterIndex = 0;

            // Variables initialization
            direction = characterSpawnDetails.Direction;

            var characterName = characterSpawnDetails.Character.Name;
            var characterClass = characterSpawnDetails.Character.CharacterType;
            var gameObject = 
                Resources.Load<GameObject>(
                    string.Format(GameObjectsPath, characterClass));

            // Creating the character
            characterGameObject = 
                Instantiate(gameObject, Vector3.zero, Quaternion.identity, transform);

            // Sets the position
            characterGameObject.transform.localPosition =
                gameObject.transform.localPosition;

            characterGameObject.transform.SetAsFirstSibling();

            // The character name game object
            characterGameObject.name = 
                characterGameObject.name.RemoveCloneFromName();

            // The character sprite game object
            var character =
                characterGameObject.transform.GetChild(CharacterIndex);

            characterSpriteGameObject = character.gameObject;

            // Calling other methods
            InitializeCharacterName(characterName);
            InitializeSpriteRenderer();
            InitializeCharacterInformationProvider(characterSpawnDetails.Character);

            ChangeCharacterDirection();
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