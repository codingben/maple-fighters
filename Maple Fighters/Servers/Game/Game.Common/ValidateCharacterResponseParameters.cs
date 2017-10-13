using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct ValidateCharacterResponseParameters : IParameters
    {
        public ValidateCharacterStatus Status;

        public ValidateCharacterResponseParameters(ValidateCharacterStatus status)
        {
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (ValidateCharacterStatus)reader.ReadByte();
        }
    }
}