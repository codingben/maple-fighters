using System.IO;
using CommonCommunicationInterfaces;

namespace Login.Common
{
    public struct AuthenticateResponseParameters : IParameters
    {
        public LoginStatus Status;
        public string AccessToken;
        public bool HasAccessToken;

        public AuthenticateResponseParameters(LoginStatus status, string accessToken = null)
        {
            Status = status;
            AccessToken = accessToken;
            HasAccessToken = !string.IsNullOrEmpty(accessToken);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
            writer.Write(HasAccessToken);

            if (HasAccessToken)
            {
                writer.Write(AccessToken);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (LoginStatus)reader.ReadByte();
            HasAccessToken = reader.ReadBoolean();

            if (HasAccessToken)
            {
                AccessToken = reader.ReadString();
            }
        }
    }
}