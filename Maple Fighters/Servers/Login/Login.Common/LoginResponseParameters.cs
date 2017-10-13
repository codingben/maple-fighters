using System.IO;
using CommonCommunicationInterfaces;

namespace Login.Common
{
    public struct LoginResponseParameters : IParameters
    {
        public LoginStatus Status;
        public string AccessToken;
        public bool HasAccessToken;

        public LoginResponseParameters(LoginStatus status, string accessToken = null, bool hasAccessToken = false)
        {
            Status = status;
            AccessToken = accessToken;
            HasAccessToken = hasAccessToken;
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