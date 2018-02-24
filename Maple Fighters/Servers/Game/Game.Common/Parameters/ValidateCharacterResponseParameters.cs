using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct ValidateCharacterResponseParameters : IParameters
    {
        public CharacterValidationStatus Status;

        public ValidateCharacterResponseParameters(CharacterValidationStatus status)
        {
            Status = status;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (CharacterValidationStatus)reader.ReadByte();
        }
    }
}