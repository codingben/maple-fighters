using System.IO;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;

namespace Authorization.Server.Common
{
    public struct AuthorizeAccessTokenResponseParameters : IParameters
    {
        public int UserId;
        public AuthorizationStatus Status;

        public AuthorizeAccessTokenResponseParameters(int userId = 0, AuthorizationStatus status = AuthorizationStatus.Failed)
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