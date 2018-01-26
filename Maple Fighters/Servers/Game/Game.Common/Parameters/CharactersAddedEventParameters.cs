using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct CharactersAddedEventParameters : IParameters
    {
        public CharacterSpawnDetailsParameters[] CharactersSpawnDetails;

        public CharactersAddedEventParameters(CharacterSpawnDetailsParameters[] charactersSpawnDetails)
        {
            CharactersSpawnDetails = charactersSpawnDetails;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(CharactersSpawnDetails);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharactersSpawnDetails = reader.ReadArray<CharacterSpawnDetailsParameters>();
        }
    }
}