using Scripts.UI.Models;

namespace Scripts.UI.Controllers
{
    public interface IOnCharacterReceivedListener
    {
        void OnCharactersReceived();

        void OnCharacterReceived(CharacterDetails characterDetails);
    }
}