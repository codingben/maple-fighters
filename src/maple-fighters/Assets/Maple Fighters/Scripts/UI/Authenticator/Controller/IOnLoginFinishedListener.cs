namespace Scripts.UI.Authenticator
{
    public interface IOnLoginFinishedListener
    {
        void OnLoginSucceeded();

        void OnLoginFailed(string reason);
    }
}