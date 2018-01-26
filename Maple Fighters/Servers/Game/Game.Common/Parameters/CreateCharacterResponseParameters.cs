using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CreateCharacterResponseParameters : IParameters
    {
        public CharacterCreationStatus Status;

        public CreateCharacterResponseParameters(CharacterCreationStatus characterCreationStatus)
        {
            Status = characterCreationStatus;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Status);
        }

        public void Deserialize(BinaryReader reader)
        {
            Status = (CharacterCreationStatus) reader.ReadByte();
        }
    }
}