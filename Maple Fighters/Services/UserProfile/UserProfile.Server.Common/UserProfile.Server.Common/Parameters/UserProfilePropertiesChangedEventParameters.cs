using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct UserProfilePropertiesChangedEventParameters : IParameters
    {
        public int UserId;
        public ServerType ServerType;
        public ConnectionStatus ConnectionStatus;

        public UserProfilePropertiesChangedEventParameters(int userId, ServerType serverType, ConnectionStatus connectionStatus)
        {
            UserId = userId;
            ServerType = serverType;
            ConnectionStatus = connectionStatus;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
            writer.Write((byte)ServerType);
            writer.Write((byte)ConnectionStatus);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
            ServerType = (ServerType)reader.ReadByte();
            ConnectionStatus = (ConnectionStatus)reader.ReadByte();
        }
    }
}