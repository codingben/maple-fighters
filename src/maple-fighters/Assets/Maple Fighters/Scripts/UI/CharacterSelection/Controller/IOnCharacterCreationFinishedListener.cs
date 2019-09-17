namespace Scripts.UI.Controllers
{
    public interface IOnCharacterCreationFinishedListener
    {
        void OnCharacterCreated();

        void OnCreateCharacterFailed(CharacterCreationFailed reason);
    }
}