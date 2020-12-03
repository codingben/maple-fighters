using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct ValidateCharacterResponseParameters : IParameters
    {
        public CharacterValidationStatus Status;
        public Maps Map;

        public ValidateCharacterResponseParameters(CharacterValidationStatus status = CharacterValidationStatus.Wrong, Maps map = Maps.Map_1)
        {
            Status = status;
            Map = map;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);

            if (Status == CharacterValidationStatus.Ok)
            {
                writer.Write((byte)Map);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (CharacterValidationStatus)reader.ReadByte();

            if (Status == CharacterValidationStatus.Ok)
            {
                Map = (Maps)reader.ReadByte();
            }
        }
    }
}