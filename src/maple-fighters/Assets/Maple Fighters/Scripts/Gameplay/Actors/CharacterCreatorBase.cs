using System;
using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public interface ISpawnedCharacterDetails
    {
        CharacterSpawnDetailsParameters GetCharacterDetails();
    }

    public class SpawnedCharacterDetails : MonoBehaviour, ISpawnedCharacterDetails
    {
        private CharacterSpawnDetailsParameters characterDetails;

        public void SetCharacterDetails(CharacterSpawnDetailsParameters characterDetails)
        {
            this.characterDetails = characterDetails;
        }

        public CharacterSpawnDetailsParameters GetCharacterDetails()
        {
            return characterDetails;
        }
    }

    public interface ISpawnedCharacterCreator
    {
        GameObject Create(Transform parent, CharacterClasses characterClass);
    }

    public class SpawnedCharacterCreator : MonoBehaviour, ISpawnedCharacterCreator
    {
        private const string GameObjectsPath = "Game/{0}";

        public GameObject Create(Transform parent, CharacterClasses characterClass)
        {
            // Loading the character
            var path = string.Format(GameObjectsPath, characterClass);
            var characterObject = Resources.Load<GameObject>(path);

            // Creating the character
            var spawnedCharacter = 
                Instantiate(characterObject, Vector3.zero, Quaternion.identity, parent);

            // Sets the position
            spawnedCharacter.transform.localPosition = characterObject.transform.localPosition;
            spawnedCharacter.transform.SetAsFirstSibling();

            // Sets the character name
            spawnedCharacter.name = spawnedCharacter.name.RemoveCloneFromName();

            return spawnedCharacter;
        }
    }

    public interface ISpawnedCharacter
    {
        event Action CharacterSpawned;

        GameObject GetCharacterGameObject();

        GameObject GetCharacterSpriteGameObject();
    }

    [RequireComponent(
        typeof(SpawnedCharacterCreator),
        typeof(SpawnedCharacterDetails))]
    public class SpawnCharacter : MonoBehaviour, ISpawnedCharacter
    {
        public event Action CharacterSpawned;

        private GameObject spawnedCharacter;

        private ISpawnedCharacterCreator spawnedCharacterCreator;
        private ISpawnedCharacterDetails spawnedCharacterDetails;

        private void Awake()
        {
            spawnedCharacterCreator = GetComponent<ISpawnedCharacterCreator>();
            spawnedCharacterDetails = GetComponent<ISpawnedCharacterDetails>();
        }

        public void Spawn()
        {
            var characterDetails =
                spawnedCharacterDetails.GetCharacterDetails();
            var characterClass = characterDetails.Character.CharacterType;

            spawnedCharacter =
                spawnedCharacterCreator.Create(parent: transform, characterClass);

            CharacterSpawned?.Invoke();
        }

        public GameObject GetCharacterGameObject()
        {
            return spawnedCharacter;
        }

        public GameObject GetCharacterSpriteGameObject()
        {
            const int CharacterIndex = 0;

            var characterSprite =
                spawnedCharacter?.transform.GetChild(CharacterIndex);

            return characterSprite?.gameObject;
        }
    }

    [RequireComponent(typeof(SpawnCharacter), typeof(SpawnedCharacterDetails))]
    public class CharacterNameInitializer : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderIndex;

        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
        }

        private void Start()
        {
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnDestroy()
        {
            spawnedCharacter.CharacterSpawned -= OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var characterNameSetter = spawnedCharacter
                .GetCharacterSpriteGameObject()
                .GetComponent<CharacterNameSetter>();
            if (characterNameSetter != null)
            {
                var characterDetailsProvider = GetComponent<ISpawnedCharacterDetails>();
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

        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
        }

        private void Start()
        {
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnDestroy()
        {
            spawnedCharacter.CharacterSpawned -= OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var spriteRenderer = spawnedCharacter
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
        typeof(SpawnedCharacterDetails))]
    public class CharacterInformationInitializer : MonoBehaviour
    {
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
        }

        private void Start()
        {
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnDestroy()
        {
            spawnedCharacter.CharacterSpawned -= OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var characterInfoProvider = GetComponent<CharacterInformationProvider>();
            if (characterInfoProvider != null)
            {
                var characterDetailsProvider = GetComponent<ISpawnedCharacterDetails>();
                var characterDetails = characterDetailsProvider.GetCharacterDetails();
                var character = characterDetails.Character;

                characterInfoProvider.SetCharacterInformation(character);
            }
        }
    }

    [RequireComponent(typeof(SpawnCharacter), typeof(SpawnedCharacterDetails))]
    public class CharacterDirectionSetter : MonoBehaviour
    {
        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
        }

        private void Start()
        {
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnDestroy()
        {
            spawnedCharacter.CharacterSpawned -= OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var characterInfoProvider = GetComponent<CharacterInformationProvider>();
            if (characterInfoProvider != null)
            {
                var characterDetailsProvider = GetComponent<ISpawnedCharacterDetails>();
                var characterDetails = characterDetailsProvider.GetCharacterDetails();
                var direction = characterDetails.Direction;

                const float Scale = 1;

                var transform = 
                    spawnedCharacter.GetCharacterGameObject().transform;

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