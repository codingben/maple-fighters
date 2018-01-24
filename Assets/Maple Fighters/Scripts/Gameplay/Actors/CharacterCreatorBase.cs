using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterCreatorBase : MonoBehaviour, ICharacterCreator
    {
        [Header("Sprite")]
        public int OrderInLayer;

        protected GameObject character;
        protected GameObject characterSprite;

        protected Directions direction { get; private set; }

        public virtual void Create(CharacterSpawnDetails characterSpawnDetails)
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

            characterSprite = character.transform.GetChild(CHARACTER_INDEX).gameObject;

            InitializeCharacterName(characterName);
            InitializeSpriteRenderer();
            InitializeCharacterInformationProvider(characterSpawnDetails.Character);
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

        private void InitializeCharacterInformationProvider(CharacterFromDatabase characterInformation)
        {
            var characterInformationProvider = GetComponent<CharacterInformationProvider>();
            characterInformationProvider.SetCharacterInformation(characterInformation);
        }
    }
}