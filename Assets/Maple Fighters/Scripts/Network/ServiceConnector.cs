using System.Collections.Generic;
using System.Threading.Tasks;
using CommonTools.Coroutines;
using Scripts.Utils;
using WaitForSeconds = CommonTools.Coroutines.WaitForSeconds;

namespace Scripts.Services
{
    public class ServiceConnector<T> : DontDestroyOnLoad<T>
        where T : ServiceConnector<T>
    {
        private const int AUTO_TIME_FOR_DISCONNECT = 60;

        private IServiceBase serviceBase;
        private ICoroutine disconnectAutomatically;

        protected readonly ExternalCoroutinesExecutor CoroutinesExecutor = new ExternalCoroutinesExecutor();

        private void Update()
        {
            CoroutinesExecutor.Update();
        }

        private void OnDestroy()
        {
            OnDestroyed();
        }

        protected virtual void OnDestroyed()
        {
            CoroutinesExecutor.Dispose();
        }

        protected async Task<ConnectionStatus> Connect(IYield yield, IServiceBase serviceBase, ConnectionInformation connectionInformation)
        {
            this.serviceBase = serviceBase;

            var connectionStatus = await serviceBase.Connect(yield, CoroutinesExecutor, connectionInformation);
            return connectionStatus;
        }

        protected void DisconnectAutomatically()
        {
            if (disconnectAutomatically == null)
            {
                disconnectAutomatically = CoroutinesExecutor.StartCoroutine(DisconnectAutomaticallyRoutine());
            }
        }

        protected IEnumerator<IYieldInstruction> DisconnectAutomaticallyRoutine()
        {
            yield return new WaitForSeconds(AUTO_TIME_FOR_DISCONNECT);
            Disconnect();
        }

        protected void Disconnect()
        {
            disconnectAutomatically?.Dispose();
            disconnectAutomatically = null;

            serviceBase?.Dispose();
        }

        private void OnApplicationQuit()
        {
            Disconnect();
        }

        protected bool IsConnected()
        {
            return serviceBase != null && serviceBase.IsConnected();
        }
    }
}