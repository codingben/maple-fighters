namespace Scripts.UI.Authenticator
{
    public interface IOnLoginFinishedListener
    {
        void OnLoginSucceed();

        void OnLoginFailed(string reason);
    }
}