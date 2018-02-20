using System.IO;
using CommonCommunicationInterfaces;

namespace Characters.Common
{
    public struct FetchCharacterRequestParameters : IParameters
    {
        public int UserId;
        public int CharacterIndex;

        public FetchCharacterRequestParameters(int userId, int characterIndex)
        {
            UserId = userId;
            CharacterIndex = characterIndex;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
            writer.Write(CharacterIndex);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
            CharacterIndex = reader.ReadInt32();
        }
    }
}