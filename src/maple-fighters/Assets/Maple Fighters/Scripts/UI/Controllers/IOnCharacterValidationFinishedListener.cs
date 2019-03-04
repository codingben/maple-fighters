namespace Scripts.UI.Controllers
{
    public interface IOnCharacterValidationFinishedListener
    {
        void OnCharacterValidated(UIMapIndex uiMapIndex);

        void OnCharacterUnvalidated();
    }
}