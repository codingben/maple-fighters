using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterInformationProvider : MonoBehaviour
    {
        private CharacterFromDatabase character;

        public void SetCharacterInformation(CharacterFromDatabase character)
        {
            this.character = character;
        }

        public CharacterFromDatabase GetCharacterInfo()
        {
            return character;
        }
    }
}