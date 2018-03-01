using System;
using System.Threading.Tasks;
using CommonCommunicationInterfaces;
using CommonTools.Coroutines;
using CommonTools.Log;
using ExitGames.Client.Photon;

namespace PhotonClientImplementation
{
    public class PhotonServerConnector
    {
        private readonly Func<ICoroutinesExecutor> coroutinesExecuterProvider;

        public PhotonServerConnector(Func<ICoroutinesExecutor> coroutinesExecuterProvider)
        {
            this.coroutinesExecuterProvider = coroutinesExecuterProvider;
        }

        public async Task<IServerPeer> ConnectAsync(IYield yield, PeerConnectionInformation connectionInformation, ConnectionDetails connectionDetails)
        {
            var coroutinesExecuter = coroutinesExecuterProvider.Invoke();
            var photonPeer = new PhotonPeer(connectionInformation, connectionDetails.ConnectionProtocol, connectionDetails.DebugLevel, coroutinesExecuter);
            photonPeer.Connect();

            var statusCode = await WaitForStatusCodeChange(yield, photonPeer);
            if (statusCode == StatusCode.Connect)
            {
                return photonPeer;
            }

            LogUtils.Log($"Connecting to {connectionInformation.Ip}:{connectionInformation.Port} failed. Status Code: {statusCode}");
            return null;
        }

        private async Task<StatusCode> WaitForStatusCodeChange(IYield yield, PhotonPeer photonPeer)
        {
            var statusChanged = false;
            var statusCode = StatusCode.Disconnect;

            Action<StatusCode> onStatusChanged = (newStatusCode) =>
            {
                statusChanged = true;
                statusCode = newStatusCode;
            };

            photonPeer.StatusChanged += onStatusChanged;

            await yield.Return(new WaitUntilIsTrue(() => statusChanged));

            photonPeer.StatusChanged -= onStatusChanged;

            return statusCode;
        }
    }
}