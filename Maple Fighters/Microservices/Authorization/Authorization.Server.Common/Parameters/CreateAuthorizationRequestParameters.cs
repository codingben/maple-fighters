using System.IO;
using CommonCommunicationInterfaces;

namespace Authorization.Server.Common
{
    public struct CreateAuthorizationRequestParameters : IParameters
    {
        public int UserId;

        public CreateAuthorizationRequestParameters(int userId)
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