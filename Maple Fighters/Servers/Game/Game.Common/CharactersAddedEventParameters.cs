using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct CharactersAddedEventParameters : IParameters
    {
        public CharacterInformation[] CharactersInformation;

        public CharactersAddedEventParameters(CharacterInformation[] charactersInformation)
        {
            CharactersInformation = charactersInformation;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(CharactersInformation);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharactersInformation = reader.ReadArray<CharacterInformation>();
        }
    }
}