using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterInformationProvider : MonoBehaviour
    {
        private CharacterParameters character;

        public void SetCharacterInformation(CharacterParameters character)
        {
            this.character = character;
        }

        public CharacterParameters GetCharacterInfo()
        {
            return character;
        }
    }
}