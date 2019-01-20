using CommonTools.Log;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    [RequireComponent(typeof(CharacterInformationProvider))]
    public class CharacterCreatorBase : MonoBehaviour, ICharacterCreator
    {
        public GameObject Character => characterGameObject;

        [Header("Sprite"), SerializeField]
        protected int orderInLayer;

        protected GameObject characterGameObject;
        protected GameObject characterSpriteGameObject;

        protected Directions Direction { get; private set; }

        public virtual void Create(
            CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            const string GameObjectsPath = "Game/{0}";
            const int CharacterIndex = 0;

            Direction = characterSpawnDetails.Direction;

            var characterName = characterSpawnDetails.Character.Name;
            var characterClass = characterSpawnDetails.Character.CharacterType;

            var gameObject = 
                Resources.Load<GameObject>(
                    string.Format(GameObjectsPath, characterClass));
            characterGameObject = 
                Instantiate(gameObject, Vector3.zero, Quaternion.identity, transform);
            characterGameObject.transform.localPosition =
                gameObject.transform.localPosition;
            characterGameObject.transform.SetAsFirstSibling();
            characterGameObject.name = 
                characterGameObject.name.RemoveCloneFromName();

            var character =
                characterGameObject.transform.GetChild(CharacterIndex);
            characterSpriteGameObject = character.gameObject;

            InitializeCharacterName(characterName);
            InitializeSpriteRenderer();
            InitializeCharacterInformationProvider(characterSpawnDetails.Character);

            ChangeCharacterDirection();
        }

        private void InitializeCharacterName(string characterName)
        {
            var characterNameComponent =
                characterSpriteGameObject.GetComponent<CharacterNameSetter>()
                    .AssertNotNull();
            characterNameComponent.SetName(characterName);
            characterNameComponent.SetSortingOrder(orderInLayer);
        }

        private void InitializeSpriteRenderer()
        {
            var spriteRenderer = 
                characterSpriteGameObject.GetComponent<SpriteRenderer>()
                    .AssertNotNull();
            spriteRenderer.sortingOrder = orderInLayer;
        }

        private void InitializeCharacterInformationProvider(
            CharacterParameters character)
        {
            var characterInformationProvider = 
                GetComponent<CharacterInformationProvider>().AssertNotNull();
            characterInformationProvider.SetCharacterInformation(character);
        }

        private void ChangeCharacterDirection()
        {
            const float Scale = 1;

            var transform = characterGameObject.transform;

            switch (Direction)
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