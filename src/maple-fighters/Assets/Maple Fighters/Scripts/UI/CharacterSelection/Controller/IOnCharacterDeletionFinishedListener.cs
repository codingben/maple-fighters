namespace Scripts.UI.Controllers
{
    public interface IOnCharacterDeletionFinishedListener
    {
        void OnCharacterDeletionSucceed();

        void OnCharacterDeletionFailed();
    }
}