using System.IO;
using CommonCommunicationInterfaces;

namespace Login.Common
{
    public struct LoginResponseParameters : IParameters
    {
        public LoginStatus Status;

        public LoginResponseParameters(LoginStatus status)
        {
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (LoginStatus)reader.ReadByte();
        }
    }
}