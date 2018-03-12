using System.IO;
using CommonCommunicationInterfaces;

namespace UserProfile.Server.Common
{
    public struct CreateUserProfileRequestParameters : IParameters
    {
        public int UserId;

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
        }
    }
}