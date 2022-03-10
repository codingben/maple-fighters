using UnityEngine;

namespace Scripts.Gameplay.Player
{
    [RequireComponent(typeof(SpawnCharacter), typeof(CharacterDataProvider))]
    public class CharacterNameInitializer : MonoBehaviour
    {
        [SerializeField]
        private int sortingOrderIndex;

        private ISpawnedCharacter spawnedCharacter;

        private void Awake()
        {
            spawnedCharacter = GetComponent<ISpawnedCharacter>();
            spawnedCharacter.CharacterSpawned += OnCharacterSpawned;
        }

        private void OnCharacterSpawned()
        {
            var characterNameSetter = spawnedCharacter
                .GetCharacterSprite()
                .GetComponent<CharacterNameSetter>();
            if (characterNameSetter != null)
            {
                var characterDataProvider = GetComponent<ICharacterDataProvider>();
                var characterData = characterDataProvider.GetCharacterData();
                var name = characterData.Name;

                characterNameSetter.SetName(name);
                characterNameSetter.SetSortingOrder(sortingOrderIndex);
            }
        }
    }
}