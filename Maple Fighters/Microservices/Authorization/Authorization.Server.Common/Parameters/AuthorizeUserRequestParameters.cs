using System.IO;
using CommonCommunicationInterfaces;

namespace Authorization.Server.Common
{
    public struct AuthorizeUserRequestParameters : IParameters
    {
        public int UserId;

        public AuthorizeUserRequestParameters(int userId)
        {
            UserId = userId;
        }

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