namespace Scripts.UI.Controllers
{
    public interface ICharacterImageCollection
    {
        void Set(UICharacterIndex uiCharacterIndex, IClickableCharacterView clickableCharacterView);

        IClickableCharacterView Get(UICharacterIndex uiCharacterIndex);

        IClickableCharacterView[] GetAll();
    }
}