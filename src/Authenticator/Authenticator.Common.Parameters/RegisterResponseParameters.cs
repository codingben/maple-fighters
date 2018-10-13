using System.IO;
using Authenticator.Common.Enums;
using CommonCommunicationInterfaces;

namespace Authenticator.Common.Parameters
{
    public struct RegisterResponseParameters : IParameters
    {
        public RegistrationStatus RegistrationStatus;

        public RegisterResponseParameters(RegistrationStatus registrationStatus)
        {
            RegistrationStatus = registrationStatus;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)RegistrationStatus);
        }

        public void Deserialize(BinaryReader reader)
        {
            RegistrationStatus = (RegistrationStatus)reader.ReadByte();
        }
    }
}