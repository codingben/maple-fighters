using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CharacterAddedEventParameters : IParameters
    {
        public CharacterSpawnDetailsParameters CharacterSpawnDetails;

        public CharacterAddedEventParameters(CharacterSpawnDetailsParameters characterSpawnDetails)
        {
            CharacterSpawnDetails = characterSpawnDetails;
        }

        public void Serialize(BinaryWriter writer)
        {
            CharacterSpawnDetails.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharacterSpawnDetails.Deserialize(reader);
        }
    }
}