namespace Scripts.UI.Controllers
{
    public interface IChatListener
    {
        void OnMessageReceived(string message);
    }
}