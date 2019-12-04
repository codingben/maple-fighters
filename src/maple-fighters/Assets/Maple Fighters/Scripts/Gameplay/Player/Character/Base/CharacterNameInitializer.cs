using UnityEngine;

namespace Scripts.Gameplay.Player
{
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
}