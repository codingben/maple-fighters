namespace Scripts.UI.CharacterSelection
{
    public interface IOnCharacterValidationFinishedListener
    {
        void OnCharacterValidated(string mapName);

        void OnCharacterUnvalidated();
    }
}