namespace Scripts.UI.Controllers
{
    public interface IOnRegistrationFinishedListener
    {
        void OnRegistrationSucceed();

        void OnEmailExistsError();
    }
}