using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class CharacterDataProvider : MonoBehaviour, ISpawnedCharacterDetails
    {
        private CharacterData characterData;

        public void SetCharacterDetails(CharacterData characterData)
        {
            this.characterData = characterData;
        }

        public CharacterData GetCharacterData()
        {
            return characterData;
        }
    }
}