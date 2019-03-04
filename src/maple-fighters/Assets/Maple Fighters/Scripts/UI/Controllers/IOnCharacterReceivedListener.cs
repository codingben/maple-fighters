using Scripts.UI.Models;

namespace Scripts.UI.Controllers
{
    public interface IOnCharacterReceivedListener
    {
        void OnBeforeCharacterReceived();

        void OnAfterCharacterReceived(CharacterDetails characterDetails);
    }
}