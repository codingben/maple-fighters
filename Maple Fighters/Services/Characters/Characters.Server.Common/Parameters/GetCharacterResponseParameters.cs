using System.IO;
using Characters.Client.Common;
using CommonCommunicationInterfaces;

namespace Characters.Server.Common
{
    public struct GetCharacterResponseParameters : IParameters
    {
        public CharacterFromDatabaseParameters? Character;

        public GetCharacterResponseParameters(CharacterFromDatabaseParameters? character)
        {
            Character = character;
        }

        public void Serialize(BinaryWriter writer)
        {
            Character?.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            Character?.Deserialize(reader);
        }
    }
}