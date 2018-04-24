using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreatorBase : MonoBehaviour, ICharacterCreator
    {
        public GameObject Character => character;

        [Header("Sprite")]
        [SerializeField] protected int OrderInLayer;

        protected GameObject character;
        protected GameObject characterSprite;

        protected Directions direction { get; private set; }

        public virtual void Create(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            const string GAME_OBJECTS_PATH = "Game/{0}";
            const int CHARACTER_INDEX = 0;

            direction = characterSpawnDetails.Direction;

            var characterName = characterSpawnDetails.Character.Name;
            var characterClass = characterSpawnDetails.Character.CharacterType;

            var gameObject = Resources.Load<GameObject>(string.Format(GAME_OBJECTS_PATH, characterClass));
            character = Instantiate(gameObject, Vector3.zero, Quaternion.identity, transform);
            character.transform.localPosition = gameObject.transform.localPosition;
            character.transform.SetAsFirstSibling();
            character.name = character.name.RemoveCloneFromName();

            characterSprite = character.transform.GetChild(CHARACTER_INDEX).gameObject;

            InitializeCharacterName(characterName);
            InitializeSpriteRenderer();
            InitializeCharacterInformationProvider(characterSpawnDetails.Character);

            ChangeCharacterDirection();
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameComponent = characterSprite.GetComponent<CharacterName>();
            characterNameComponent.SetName(characterName);
            characterNameComponent.SetSortingOrder(OrderInLayer);
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer = characterSprite.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = OrderInLayer;
        }

        private void InitializeCharacterInformationProvider(CharacterParameters character)
        {
            var characterInformationProvider = GetComponent<CharacterInformationProvider>();
            characterInformationProvider.SetCharacterInformation(character);
        }

        private void ChangeCharacterDirection()
        {
            const float SCALE = 1;

            var transform = character.transform;

            switch (direction)
            {
                case Directions.Left:
                {
                    transform.localScale = new Vector3(SCALE, transform.localScale.y, transform.localScale.z);
                    break;
                }
                case Directions.Right:
                {
                    transform.localScale = new Vector3(-SCALE, transform.localScale.y, transform.localScale.z);
                    break;
                }
            }
        }
    }
}