namespace Scripts.UI.CharacterSelection
{
    public interface IOnCharacterValidationFinishedListener
    {
        void OnCharacterValidated();

        void OnCharacterUnvalidated();
    }
}