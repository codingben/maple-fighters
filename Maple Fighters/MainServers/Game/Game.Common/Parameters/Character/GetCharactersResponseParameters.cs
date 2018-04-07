using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Game.Common
{
    public struct GetCharactersResponseParameters : IParameters
    {
        public CharacterParameters[] Characters;

        public GetCharactersResponseParameters(CharacterParameters[] characters)
        {
            Characters = characters;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Characters != null && Characters.Length > 0);

            if (Characters?.Length > 0)
            {
                writer.WriteArray(Characters);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            var hasArray = reader.ReadBoolean();
            if (hasArray)
            {
                Characters = reader.ReadArray<CharacterParameters>();
            }
        }
    }
}