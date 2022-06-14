namespace Scripts.UI.Authenticator
{
    public interface IOnRegistrationFinishedListener
    {
        void OnRegistrationSucceeded();

        void OnRegistrationFailed(string reason);
    }
}