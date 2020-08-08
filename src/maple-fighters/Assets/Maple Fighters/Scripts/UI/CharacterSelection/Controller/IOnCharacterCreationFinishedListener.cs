namespace Scripts.UI.CharacterSelection
{
    public interface IOnCharacterCreationFinishedListener
    {
        void OnCharacterCreated();

        void OnCreateCharacterFailed(UICharacterCreationFailed reason);
    }
}