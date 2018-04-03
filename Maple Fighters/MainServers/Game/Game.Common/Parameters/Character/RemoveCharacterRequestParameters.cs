using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct RemoveCharacterRequestParameters : IParameters
    {
        public int CharacterIndex;

        public RemoveCharacterRequestParameters(int characterIndex)
        {
            CharacterIndex = characterIndex;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(CharacterIndex);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharacterIndex = reader.ReadInt32();
        }
    }
}