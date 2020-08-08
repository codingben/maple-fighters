using UnityEngine;

namespace Scripts.Gameplay.Player
{
    public class SpawnedCharacterDetails : MonoBehaviour, ISpawnedCharacterDetails
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