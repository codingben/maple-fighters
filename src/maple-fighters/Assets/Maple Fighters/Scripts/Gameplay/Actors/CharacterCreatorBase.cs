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

    public class SpawnCharacter : MonoBehaviour, ISpawnedCharacter
    {
        public event Action CharacterSpawned;

        private GameObject spawnedCharacter;

        private ICharacterGameObjectCreator characterCreator;
        private ICharacterDetailsProvider characterDetailsProvider;

        private void Awake()
        {
            characterCreator = GetComponent<ICharacterGameObjectCreator>();
            characterDetailsProvider =
                GetComponent<ICharacterDetailsProvider>();
        }

        public void Spawn()
        {
            var characterDetails = characterDetailsProvider.GetCharacterDetails();
            var characterClass = characterDetails.Character.CharacterType;

            spawnedCharacter = 
                characterCreator.CreateCharacter(parent: transform, characterClass);

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

    [RequireComponent(typeof(SpawnCharacter), typeof(CharacterDetails))]
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
        typeof(CharacterDetails))]
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
                var characterDetailsProvider = GetComponent<ICharacterDetailsProvider>();
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