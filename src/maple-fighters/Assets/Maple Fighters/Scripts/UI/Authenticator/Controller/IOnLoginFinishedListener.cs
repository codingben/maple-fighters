namespace Scripts.UI.Authenticator
{
    public interface IOnLoginFinishedListener
    {
        void OnLoginSucceed();

        void OnLoginFailed();

        void OnInvalidEmailError();

        void OnInvalidPasswordError();

        void OnNonAuthorizedError();
    }
}