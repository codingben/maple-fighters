using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Character.Client.Common
{
    public struct GetCharactersResponseParameters : IParameters
    {
        public CharacterFromDatabaseParameters[] Characters;

        public GetCharactersResponseParameters(CharacterFromDatabaseParameters[] characters)
        {
            Characters = characters;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(Characters);
        }

        public void Deserialize(BinaryReader reader)
        {
            Characters = reader.ReadArray<CharacterFromDatabaseParameters>();
        }
    }
}