namespace Scripts.UI.GameServerBrowser
{
    public struct UIGameServerButtonData
    {
        public string IP { get; }

        public string ServerName { get; }

        public int Port { get; }

        public int Connections { get; }

        public int MaxConnections { get; }

        public UIGameServerButtonData(
            string ip,
            string serverName,
            int port,
            int connections,
            int maxConnections)
        {
            IP = ip;
            ServerName = serverName;
            Port = port;
            Connections = connections;
            MaxConnections = maxConnections;
        }
    }
}
