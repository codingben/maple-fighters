namespace GameServerProvider.Service.Application.Components
{
    public static class Utils
    {
        public static GameServerInformation FromGameServerInformationParameters(dynamic parameters)
        {
            var name = (string)parameters.Name;
            var ip = (string)parameters.IP;
            var port = (int)parameters.Port;
            var connections = (int)parameters.Connections;
            var maxConnections = (int)parameters.MaxConnections;
            return new GameServerInformation(name, ip, port, connections, maxConnections);
        }
    }
}