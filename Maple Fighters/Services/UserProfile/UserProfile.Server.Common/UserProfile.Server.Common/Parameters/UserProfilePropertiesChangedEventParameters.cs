using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct UserProfilePropertiesChangedEventParameters : IParameters
    {
        public ServerType ServerType;
        public ConnectionStatus ConnectionStatus;

        public UserProfilePropertiesChangedEventParameters(ServerType serverType, ConnectionStatus connectionStatus)
        {
            ServerType = serverType;
            ConnectionStatus = connectionStatus;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)ServerType);
            writer.Write((byte)ConnectionStatus);
        }

        public void Deserialize(BinaryReader reader)
        {
            ServerType = (ServerType)reader.ReadByte();
            ConnectionStatus = (ConnectionStatus)reader.ReadByte();
        }
    }
}