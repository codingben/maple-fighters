using ComponentModel.Common;

namespace PhotonControl
{
    internal interface IServersController : IExposableComponent
    {
        void StartServer(string serverName, bool notify = true);
        void StopServer(string serverName, bool notify = true);

        void StartAllServers();
        void StopAllServers();
    }
}