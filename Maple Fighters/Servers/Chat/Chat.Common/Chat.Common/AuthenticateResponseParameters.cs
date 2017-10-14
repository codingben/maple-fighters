using System.IO;
using CommonCommunicationInterfaces;

namespace Chat.Common
{
    public struct AuthenticateResponseParameters : IParameters
    {
        public AuthenticateStatus Status;

        public AuthenticateResponseParameters(AuthenticateStatus status)
        {
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (AuthenticateStatus)reader.ReadByte();
        }
    }
}