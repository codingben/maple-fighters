using System;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface ICharacterDetailsProvider
    {
        CharacterSpawnDetailsParameters GetCharacterDetails();
    }

    public class CharacterDetails : MonoBehaviour, ICharacterDetailsProvider
    {
        private CharacterSpawnDetailsParameters characterSpawnDetails;

        public void SetCharacterSpawnDetails(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            this.characterSpawnDetails = characterSpawnDetails;
        }

        public CharacterSpawnDetailsParameters GetCharacterDetails()
        {
            return characterSpawnDetails;
        }
    }

    public interface ICharacterGameObjectProvider
    {
        GameObject GetCharacterGameObject();

        GameObject GetCharacterSpriteGameObject();
    }

    public interface ICharacterGameObjectCreator
    {
        GameObject CreateCharacter(Transform parent, CharacterClasses characterClass);
    }

    public class CharacterGameObjectCreator : MonoBehaviour, ICharacterGameObjectCreator
    {
        private const string GameObjectsPath = "Game/{0}";

        public GameObject CreateCharacter(Transform parent, CharacterClasses characterClass)
        {
            // Loading the character
            var path = string.Format(GameObjectsPath, characterClass);
            var characterObject = Resources.Load<GameObject>(path);

            // Creating the character
            var characterGameObject = 
                Instantiate(characterObject, Vector3.zero, Quaternion.identity, parent);

            // Sets the position
            characterGameObject.transform.localPosition = characterObject.transform.localPosition;
            characterGameObject.transform.SetAsFirstSibling();

            // Sets the character name
            characterGameObject.name = characterGameObject.name.RemoveCloneFromName();

            return characterGameObject;
        }
    }

    public class CharacterGameObject : MonoBehaviour, ICharacterGameObjectProvider
    {
        public event Action<ICharacterGameObjectProvider> CharacterCreated;

        private GameObject characterGameObject;

        private ICharacterGameObjectCreator characterCreator;
        private ICharacterDetailsProvider characterDetailsProvider;

        private void Awake()
        {
            characterCreator = GetComponent<ICharacterGameObjectCreator>();
            characterDetailsProvider =
                GetComponent<ICharacterDetailsProvider>();
        }

        public void CreateCharacter()
        {
            var characterDetails = characterDetailsProvider.GetCharacterDetails();
            var characterClass = characterDetails.Character.CharacterType;

            characterGameObject = 
                characterCreator.CreateCharacter(parent: transform, characterClass);

            CharacterCreated?.Invoke(this);
        }

        public GameObject GetCharacterGameObject()
        {
            return characterGameObject;
        }

        public GameObject GetCharacterSpriteGameObject()
        {
            const int CharacterIndex = 0;

            var characterSprite =
                characterGameObject?.transform.GetChild(CharacterIndex);

            return characterSprite?.gameObject;
        }
    }

    [RequireComponent(typeof(CharacterGameObject), typeof(CharacterDetails))]
    public class CharacterNameInitializer : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderIndex;

        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var characterNameSetter = characterGameObjectProvider
                .GetCharacterSpriteGameObject()
                .GetComponent<CharacterNameSetter>();
            if (characterNameSetter != null)
            {
                var characterDetailsProvider = GetComponent<ICharacterDetailsProvider>();
                var characterDetails = characterDetailsProvider.GetCharacterDetails();
                var characterName = characterDetails.Character.Name;

                characterNameSetter.SetName(characterName);
                characterNameSetter.SetSortingOrder(sortingOrderIndex);
            }
        }
    }

    [RequireComponent(typeof(CharacterGameObject))]
    public class SpriteRendererInitializer : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderIndex;

        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var spriteRenderer = characterGameObjectProvider
                .GetCharacterSpriteGameObject()
                .GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = sortingOrderIndex;
            }
        }
    }

    [RequireComponent(
        typeof(CharacterGameObject), 
        typeof(CharacterInformationProvider), 
        typeof(CharacterDetails))]
    public class CharacterInformationInitializer : MonoBehaviour
    {
        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var characterInfoProvider = GetComponent<CharacterInformationProvider>();
            if (characterInfoProvider != null)
            {
                var characterDetailsProvider = GetComponent<ICharacterDetailsProvider>();
                var characterDetails = characterDetailsProvider.GetCharacterDetails();
                var character = characterDetails.Character;

                characterInfoProvider.SetCharacterInformation(character);
            }
        }
    }

    [RequireComponent(typeof(CharacterGameObject), typeof(CharacterDetails))]
    public class CharacterDirectionSetter : MonoBehaviour
    {
        private CharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<CharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated(ICharacterGameObjectProvider characterGameObjectProvider)
        {
            var characterInfoProvider = GetComponent<CharacterInformationProvider>();
            if (characterInfoProvider != null)
            {
                var characterDetailsProvider = GetComponent<ICharacterDetailsProvider>();
                var characterDetails = characterDetailsProvider.GetCharacterDetails();
                var direction = characterDetails.Direction;

                const float Scale = 1;

                var transform = 
                    characterGameObjectProvider
                        .GetCharacterGameObject().transform;

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
}