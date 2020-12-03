using System.IO;
using CommonCommunicationInterfaces;

namespace Authorization.Server.Common
{
    public struct RemoveAuthorizationRequestParameters : IParameters
    {
        public int UserId;

        public RemoveAuthorizationRequestParameters(int userId)
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