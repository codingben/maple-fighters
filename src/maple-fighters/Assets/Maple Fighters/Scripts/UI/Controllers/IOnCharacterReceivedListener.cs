using Scripts.UI.Models;

namespace Scripts.UI.Controllers
{
    public interface IOnCharacterReceivedListener
    {
        void OnCharacterReceived(CharacterDetails characterDetails);
    }
}