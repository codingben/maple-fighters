namespace Scripts.UI
{
    public struct UIGameServerButtonData
    {
        public string ServerName { get; }

        public int Connections { get; }

        public int MaxConnections { get; }

        public UIGameServerButtonData(
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
