using UI.Manager;

namespace Scripts.UI.Models
{
    public class CharacterSelectionDetails : Singleton<CharacterSelectionDetails>
    {
        private CharacterDetails characterDetails;

        public void SetCharacterDetails(CharacterDetails characterDetails)
        {
            this.characterDetails = characterDetails;
        }

        public CharacterDetails GetCharacterDetails()
        {
            return characterDetails;
        }
    }
}