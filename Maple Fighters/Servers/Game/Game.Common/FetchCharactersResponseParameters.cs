using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct FetchCharactersResponseParameters : IParameters
    {
        public Character[] Characters;

        public FetchCharactersResponseParameters(Character[] characters)
        {
            Characters = characters;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(Characters);
        }

        public void Deserialize(BinaryReader reader)
        {
            Characters = reader.ReadArray<Character>();
        }
    }
}