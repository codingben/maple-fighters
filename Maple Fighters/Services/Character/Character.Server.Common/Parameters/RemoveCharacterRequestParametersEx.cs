using System.IO;
using CommonCommunicationInterfaces;

namespace Character.Server.Common
{
    public struct RemoveCharacterRequestParametersEx : IParameters
    {
        public int UserId;
        public int CharacterIndex;

        public RemoveCharacterRequestParametersEx(int userId, int characterIndex)
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