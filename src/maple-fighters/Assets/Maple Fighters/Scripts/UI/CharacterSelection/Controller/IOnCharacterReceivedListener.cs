namespace Scripts.UI.CharacterSelection
{
    public interface IOnCharacterReceivedListener
    {
        void OnCharacterReceived(UICharacterDetails characterDetails);

        void OnAfterCharacterReceived();
    }
}