namespace Scripts.UI.Authenticator
{
    public interface IOnConnectionFinishedListener
    {
        void OnConnectionSucceed();

        void OnConnectionFailed();
    }
}