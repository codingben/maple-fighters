using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct UnsubscribeFromUserProfileRequestParameters : IParameters
    {
        public int UserId;
        public int ServerId;

        public UnsubscribeFromUserProfileRequestParameters(int userId, int serverId)
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