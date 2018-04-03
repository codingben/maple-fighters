using System.IO;
using CommonCommunicationInterfaces;

namespace GameServerProvider.Client.Common
{
    public struct GameServerInformationParameters : IParameters
    {
        public string Name;
        public string IP;
        public int Port;
        public int Connections;
        public int MaxConnections;

        public GameServerInformationParameters(string name, string ip, int port, int connections, int maxConnections)
        {
            Name = name;
            IP = ip;
            Port = port;
            Connections = connections;
            MaxConnections = maxConnections;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(IP);
            writer.Write(Port);
            writer.Write(Connections);
            writer.Write(MaxConnections);
        }

        public void Deserialize(BinaryReader reader)
        {
            Name = reader.ReadString();
            IP = reader.ReadString();
            Port = reader.ReadInt32();
            Connections = reader.ReadInt32();
            MaxConnections = reader.ReadInt32();
        }
    }
}