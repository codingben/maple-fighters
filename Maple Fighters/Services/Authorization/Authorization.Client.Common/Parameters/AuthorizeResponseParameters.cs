using System.IO;
using CommonCommunicationInterfaces;

namespace Authorization.Client.Common
{
    public struct AuthorizeResponseParameters : IParameters
    {
        public int UserId;
        public AuthorizationStatus Status;

        public AuthorizeResponseParameters(int userId, AuthorizationStatus status = AuthorizationStatus.Failed)
        {
            UserId = userId;
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
            Status = (AuthorizationStatus)reader.ReadByte();
        }
    }
}