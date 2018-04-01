namespace PhotonControl
{
    internal interface IServersController
    {
        void StartServer(string serverName, bool notify = true);
        void StopServer(string serverName, bool notify = true);

        void StartAllServers();
        void StopAllServers();
    }
}