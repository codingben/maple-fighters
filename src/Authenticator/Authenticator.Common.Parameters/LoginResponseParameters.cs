using System.IO;
using Authenticator.Common.Enums;
using CommonCommunicationInterfaces;

namespace Authenticator.Common.Parameters
{
    public struct LoginResponseParameters : IParameters
    {
        public LoginStatus LoginStatus;
        public bool IsInvalid;
        public string ErrorMessage;

        public LoginResponseParameters(
            LoginStatus loginStatus,
            string errorMessage)
        {
            LoginStatus = loginStatus;
            IsInvalid = errorMessage != null;
            ErrorMessage = errorMessage;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)LoginStatus);
            writer.Write(IsInvalid);

            if (IsInvalid)
            {
                writer.Write(ErrorMessage);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            LoginStatus = (LoginStatus)reader.ReadByte();
            IsInvalid = reader.ReadBoolean();

            if (IsInvalid)
            {
                ErrorMessage = reader.ReadString();
            }
        }
    }
}