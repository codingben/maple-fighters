using System.IO;
using CommonCommunicationInterfaces;

namespace GameServerProvider.Server.Common
{
    public struct UpdateGameServerConnectionsInfoRequestParameters : IParameters
    {
        public int Connections;

        public UpdateGameServerConnectionsInfoRequestParameters(int connections)
        {
            Connections = connections;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Connections);
        }

        public void Deserialize(BinaryReader reader)
        {
            Connections = reader.ReadInt32();
        }
    }
}