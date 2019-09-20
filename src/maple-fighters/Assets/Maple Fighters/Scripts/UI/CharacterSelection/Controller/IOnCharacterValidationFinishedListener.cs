namespace Scripts.UI.CharacterSelection
{
    public interface IOnCharacterValidationFinishedListener
    {
        void OnCharacterValidated(UIMapIndex uiMapIndex);

        void OnCharacterUnvalidated();
    }
}