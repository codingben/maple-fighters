using Scripts.UI.Models;

namespace Scripts.UI.Controllers
{
    public interface ICharacterViewListener
    {
        void CreateCharacter(CharacterDetails characterDetails);
    }
}