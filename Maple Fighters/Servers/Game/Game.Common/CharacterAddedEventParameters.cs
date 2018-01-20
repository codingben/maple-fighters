using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct CharacterAddedEventParameters : IParameters
    {
        public CharacterInformation CharacterInformation;

        public CharacterAddedEventParameters(CharacterInformation characterInformation)
        {
            CharacterInformation = characterInformation;
        }

        public void Serialize(BinaryWriter writer)
        {
            CharacterInformation.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharacterInformation.Deserialize(reader);
        }
    }
}