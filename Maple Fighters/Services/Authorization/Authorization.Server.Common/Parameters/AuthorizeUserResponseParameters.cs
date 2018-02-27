using System.IO;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;

namespace Authorization.Server.Common
{
    public struct AuthorizeUserResponseParameters : IParameters
    {
        public string AccessToken;
        public AuthorizationStatus Status;

        public AuthorizeUserResponseParameters(string accessToken, AuthorizationStatus status)
        {
            AccessToken = accessToken;
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(AccessToken);
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            AccessToken = reader.ReadString();
            Status = (AuthorizationStatus)reader.ReadByte();
        }
    }
}