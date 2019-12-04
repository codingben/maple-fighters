using Game.Common;
using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class SpawnedCharacterDetails : MonoBehaviour, ISpawnedCharacterDetails
    {
        private CharacterSpawnDetailsParameters characterDetails;

        public void SetCharacterDetails(CharacterSpawnDetailsParameters characterDetails)
        {
            this.characterDetails = characterDetails;
        }

        public CharacterSpawnDetailsParameters GetCharacterDetails()
        {
            return characterDetails;
        }
    }
}