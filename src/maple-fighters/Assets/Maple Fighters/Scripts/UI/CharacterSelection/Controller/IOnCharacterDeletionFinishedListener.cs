namespace Scripts.UI.CharacterSelection
{
    public interface IOnCharacterDeletionFinishedListener
    {
        void OnCharacterDeletionSucceed();

        void OnCharacterDeletionFailed();
    }
}