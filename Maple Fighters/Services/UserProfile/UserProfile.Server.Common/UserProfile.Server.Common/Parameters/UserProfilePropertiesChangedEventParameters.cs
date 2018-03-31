using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct UserProfilePropertiesChangedEventParameters : IParameters
    {
        public int UserId;
        public ServerType ServerType;

        public UserProfilePropertiesChangedEventParameters(int userId, ServerType serverType)
        {
            UserId = userId;
            ServerType = serverType;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
            writer.Write((byte)ServerType);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
            ServerType = (ServerType)reader.ReadByte();
        }
    }
}