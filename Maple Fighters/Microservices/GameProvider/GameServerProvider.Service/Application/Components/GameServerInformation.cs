namespace GameServerProvider.Service.Application.Components
{
    public struct GameServerInformation
    {
        public string Name;
        public string IP;
        public int Port;
        public int Connections;
        public int MaxConnections;

        public GameServerInformation(string name, string ip, int port, int connections, int maxConnections)
        {
            Name = name;
            IP = ip;
            Port = port;
            Connections = connections;
            MaxConnections = maxConnections;
        }
    }
}