namespace Scripts.UI.Controllers
{
    public interface IOnRegistrationFinishedListener
    {
        void OnRegistrationSucceed();

        void OnRegistrationFailed();

        void OnEmailExistsError();
    }
}