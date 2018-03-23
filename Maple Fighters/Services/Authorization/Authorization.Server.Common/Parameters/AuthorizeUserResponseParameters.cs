using System.IO;
using Authorization.Client.Common;
using CommonCommunicationInterfaces;

namespace Authorization.Server.Common
{
    public struct AuthorizeUserResponseParameters : IParameters
    {
        public string AccessToken;
        public AuthorizationStatus Status;

        public AuthorizeUserResponseParameters(string accessToken, AuthorizationStatus status = AuthorizationStatus.Failed)
        {
            AccessToken = accessToken;
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);

            if (Status == AuthorizationStatus.Succeed)
            {
                writer.Write(AccessToken);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (AuthorizationStatus)reader.ReadByte();

            if (Status == AuthorizationStatus.Succeed)
            {
                AccessToken = reader.ReadString();
            }
        }
    }
}