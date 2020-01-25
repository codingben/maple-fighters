using System.IO;
using CommonCommunicationInterfaces;

namespace Authenticator.Common
{
    public struct RegisterResponseParameters : IParameters
    {
        public RegistrationStatus RegistrationStatus;
        public bool IsInvalid;
        public string ErrorMessage;

        public RegisterResponseParameters(
            RegistrationStatus registrationStatus,
            string errorMessage)
        {
            RegistrationStatus = registrationStatus;
            IsInvalid = errorMessage != null;
            ErrorMessage = errorMessage;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)RegistrationStatus);
            writer.Write(IsInvalid);

            if (IsInvalid)
            {
                writer.Write(ErrorMessage);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            RegistrationStatus = (RegistrationStatus)reader.ReadByte();
            IsInvalid = reader.ReadBoolean();

            if (IsInvalid)
            {
                ErrorMessage = reader.ReadString();
            }
        }
    }
}