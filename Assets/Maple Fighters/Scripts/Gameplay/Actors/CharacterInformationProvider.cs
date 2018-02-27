using Character.Client.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterInformationProvider : MonoBehaviour
    {
        private CharacterFromDatabaseParameters character;

        public void SetCharacterInformation(CharacterFromDatabaseParameters character)
        {
            this.character = character;
        }

        public CharacterFromDatabaseParameters GetCharacterInfo()
        {
            return character;
        }
    }
}