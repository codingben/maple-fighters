using System;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface ICharacterDetailsProvider
    {
        CharacterSpawnDetailsParameters GetCharacterDetails();
    }

    // TODO: Conflicts with CharacterInformationProvider
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

    public interface ICharacterGameObject
    {
        event Action CharacterCreated;

        GameObject GetCharacterGameObject();

        GameObject GetCharacterSpriteGameObject();
    }

    // TODO: Another name needed
    public class SpawnCharacter : MonoBehaviour, ICharacterGameObject
    {
        public event Action CharacterCreated;

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

            CharacterCreated?.Invoke();
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

    [RequireComponent(typeof(SpawnCharacter), typeof(CharacterDetails))]
    public class CharacterNameInitializer : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderIndex;

        private ICharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<ICharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated()
        {
            var characterNameSetter = characterGameObject
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

    [RequireComponent(typeof(SpawnCharacter))]
    public class SpriteRendererInitializer : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderIndex;

        private ICharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<ICharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated()
        {
            var spriteRenderer = characterGameObject
                .GetCharacterSpriteGameObject()
                .GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingOrder = sortingOrderIndex;
            }
        }
    }

    [RequireComponent(
        typeof(SpawnCharacter), 
        typeof(CharacterInformationProvider), 
        typeof(CharacterDetails))]
    public class CharacterInformationInitializer : MonoBehaviour
    {
        private ICharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<ICharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated()
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

    [RequireComponent(typeof(SpawnCharacter), typeof(CharacterDetails))]
    public class CharacterDirectionSetter : MonoBehaviour
    {
        private ICharacterGameObject characterGameObject;

        private void Awake()
        {
            characterGameObject = GetComponent<ICharacterGameObject>();
        }

        private void Start()
        {
            characterGameObject.CharacterCreated += OnCharacterCreated;
        }

        private void OnDestroy()
        {
            characterGameObject.CharacterCreated -= OnCharacterCreated;
        }

        private void OnCharacterCreated()
        {
            var characterInfoProvider = GetComponent<CharacterInformationProvider>();
            if (characterInfoProvider != null)
            {
                var characterDetailsProvider = GetComponent<ICharacterDetailsProvider>();
                var characterDetails = characterDetailsProvider.GetCharacterDetails();
                var direction = characterDetails.Direction;

                const float Scale = 1;

                var transform = 
                    characterGameObject.GetCharacterGameObject().transform;

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