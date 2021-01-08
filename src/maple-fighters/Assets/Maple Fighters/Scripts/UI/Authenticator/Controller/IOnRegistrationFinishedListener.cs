namespace Scripts.UI.Authenticator
{
    public interface IOnRegistrationFinishedListener
    {
        void OnRegistrationSucceed();

        void OnRegistrationFailed(string reason);
    }
}