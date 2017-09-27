using System.IO;
using CommonCommunicationInterfaces;

namespace Registration.Common
{
    public struct RegisterResponseParameters : IParameters
    {
        public RegisterStatus Status;

        public RegisterResponseParameters(RegisterStatus status)
        {
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (RegisterStatus)reader.ReadByte();
        }
    }
}