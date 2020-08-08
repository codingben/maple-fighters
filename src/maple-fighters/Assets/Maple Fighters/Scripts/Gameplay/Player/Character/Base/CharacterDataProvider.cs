using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class CharacterDataProvider : MonoBehaviour, ICharacterDataProvider
    {
        private CharacterData characterData;

        public void SetCharacterData(CharacterData characterData)
        {
            this.characterData = characterData;
        }

        public CharacterData GetCharacterData()
        {
            return characterData;
        }
    }
}