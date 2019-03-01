namespace Scripts.UI.Controllers
{
    public interface IOnLoginFinishedListener
    {
        void OnLoginSucceed();

        void OnInvalidEmailError();

        void OnInvalidPasswordError();
    }
}