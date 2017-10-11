using CommonTools.Coroutines;
using Scripts.Containers.Service;
using Scripts.Utils;

namespace Scripts.Services
{
    public class GameConnector : DontDestroyOnLoad<GameConnector>
    {
        private readonly ExternalCoroutinesExecutor coroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Start()
        {
            ServiceContainer.GameService.Connected += OnConnected;
            ServiceContainer.GameService.Connect();
        }

        private void Update()
        {
            coroutinesExecutor?.Update();
        }

        private void OnConnected()
        {
            coroutinesExecutor.StartTask(ServiceContainer.GameService.Authenticate);
        }

        private void OnApplicationQuit()
        {
            ServiceContainer.GameService.Disconnect();
        }
    }
}