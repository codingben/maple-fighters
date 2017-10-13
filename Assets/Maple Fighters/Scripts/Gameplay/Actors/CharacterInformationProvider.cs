using Shared.Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterInformationProvider : MonoBehaviour
    {
        private CharacterInformation characterInformation;

        public void SetCharacterInformation(CharacterInformation characterInformation)
        {
            this.characterInformation = characterInformation;
        }

        public string GetCharacterName()
        {
            return characterInformation.CharacterName;
        }

        public CharacterClasses GetCharacterClasses()
        {
            return characterInformation.CharacterClass;
        }
    }
}