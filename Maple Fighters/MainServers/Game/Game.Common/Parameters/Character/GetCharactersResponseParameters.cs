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
            writer.WriteArray(Characters);
        }

        public void Deserialize(BinaryReader reader)
        {
            Characters = reader.ReadArray<CharacterParameters>();
        }
    }
}