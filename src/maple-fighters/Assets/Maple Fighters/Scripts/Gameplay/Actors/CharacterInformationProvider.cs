using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Actors
{
    public class CharacterInformationProvider : MonoBehaviour
    {
        private CharacterParameters characterParameters;

        public void SetCharacterInfo(CharacterParameters characterParameters)
        {
            this.characterParameters = characterParameters;
        }

        public CharacterParameters GetCharacterInfo()
        {
            return characterParameters;
        }
    }
}