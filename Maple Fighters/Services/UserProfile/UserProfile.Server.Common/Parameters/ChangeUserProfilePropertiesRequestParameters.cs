using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct ChangeUserProfilePropertiesRequestParameters : IParameters
    {
        public int UserId;
        public int LocalId;
        public ServerType ServerType;
        public ConnectionStatus ConnectionStatus;

        public ChangeUserProfilePropertiesRequestParameters(int userId, int localId, ServerType serverType, ConnectionStatus connectionStatus)
        {
            UserId = userId;
            LocalId = localId;
            ServerType = serverType;
            ConnectionStatus = connectionStatus;
        }

        public ChangeUserProfilePropertiesRequestParameters(int userId, ConnectionStatus connectionStatus)
        {
            UserId = userId;
            LocalId = 0;
            ServerType = ServerType.Login;
            ConnectionStatus = connectionStatus;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
            writer.Write(LocalId);
            writer.Write((byte)ServerType);
            writer.Write((byte)ConnectionStatus);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
            LocalId = reader.ReadInt32();
            ServerType = (ServerType)reader.ReadByte();
            ConnectionStatus = (ConnectionStatus)reader.ReadByte();
        }
    }
}