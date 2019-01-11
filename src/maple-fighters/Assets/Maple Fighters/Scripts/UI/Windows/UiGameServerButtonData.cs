namespace Scripts.UI
{
    public struct UiGameServerButtonData
    {
        public string ServerName { get; }

        public int Connections { get; }

        public int MaxConnections { get; }

        public UiGameServerButtonData(
            string serverName,
            int connections,
            int maxConnections)
        {
            ServerName = serverName;
            Connections = connections;
            MaxConnections = maxConnections;
        }
    }
}