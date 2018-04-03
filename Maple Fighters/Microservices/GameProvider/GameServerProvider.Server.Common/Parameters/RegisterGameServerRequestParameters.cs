using System.IO;
using CommonCommunicationInterfaces;

namespace GameServerProvider.Server.Common
{
    public struct RegisterGameServerRequestParameters : IParameters
    {
        public string Name;
        public string IP;
        public int Port;
        public int Connections;
        public int MaxConnections;

        public RegisterGameServerRequestParameters(string name, string ip, int port, int maxConnections)
        {
            Name = name;
            IP = ip;
            Port = port;
            Connections = 0;
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