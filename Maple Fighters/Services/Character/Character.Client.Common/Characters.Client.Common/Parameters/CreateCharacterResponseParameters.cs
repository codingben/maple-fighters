using System.IO;
using CommonCommunicationInterfaces;

namespace Character.Client.Common
{
    public struct CreateCharacterResponseParameters : IParameters
    {
        public CharacterCreationStatus Status;

        public CreateCharacterResponseParameters(CharacterCreationStatus characterCreationStatus = CharacterCreationStatus.Failed)
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