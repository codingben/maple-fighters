using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct SubscribeToUserProfileRequestParameters : IParameters
    {
        public int UserId;
        public int ServerId;

        public SubscribeToUserProfileRequestParameters(int userId, int serverId)
        {
            UserId = userId;
            ServerId = serverId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
            writer.Write(ServerId);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
            ServerId = reader.ReadInt32();
        }
    }
}