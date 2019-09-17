namespace Scripts.UI.Controllers
{
    public interface IOnLoginFinishedListener
    {
        void OnLoginSucceed();

        void OnLoginFailed();

        void OnInvalidEmailError();

        void OnInvalidPasswordError();
    }
}