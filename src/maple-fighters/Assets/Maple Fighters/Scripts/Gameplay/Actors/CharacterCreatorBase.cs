using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(CharacterInformationProvider))]
    public class CharacterCreatorBase : MonoBehaviour, ICharacterCreator
    {
        public GameObject Character => CharacterGameObject;

        [Header("Sprite"), SerializeField]
        protected int orderInLayer;

        protected GameObject CharacterGameObject;
        protected GameObject CharacterSpriteGameObject;

        private Directions direction;

        public virtual void Create(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            const string GameObjectsPath = "Game/{0}";
            const int CharacterIndex = 0;

            direction = characterSpawnDetails.Direction;

            var characterName = characterSpawnDetails.Character.Name;
            var characterClass = characterSpawnDetails.Character.CharacterType;
            var gameObject = 
                Resources.Load<GameObject>(
                    string.Format(GameObjectsPath, characterClass));

            CharacterGameObject = 
                Instantiate(gameObject, Vector3.zero, Quaternion.identity, transform);

            CharacterGameObject.transform.localPosition =
                gameObject.transform.localPosition;

            CharacterGameObject.transform.SetAsFirstSibling();

            CharacterGameObject.name = 
                CharacterGameObject.name.RemoveCloneFromName();

            var character =
                CharacterGameObject.transform.GetChild(CharacterIndex);
            CharacterSpriteGameObject = character.gameObject;

            InitializeCharacterName(characterName);
            InitializeSpriteRenderer();
            InitializeCharacterInformationProvider(characterSpawnDetails.Character);

            ChangeCharacterDirection();
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameSetter = CharacterSpriteGameObject
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
                CharacterSpriteGameObject.GetComponent<SpriteRenderer>();
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

            var transform = CharacterGameObject.transform;

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
    }
}