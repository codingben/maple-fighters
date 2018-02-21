using System.IO;
using CommonCommunicationInterfaces;

namespace Characters.Client.Common
{
    public struct AuthenticateResponseParameters : IParameters
    {
        public AuthenticationStatus Status;

        public AuthenticateResponseParameters(AuthenticationStatus status)
        {
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (AuthenticationStatus)reader.ReadByte();
        }
    }
}