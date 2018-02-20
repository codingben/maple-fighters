using System.IO;
using CommonCommunicationInterfaces;
using Shared.Game.Common;

namespace Characters.Common
{
    public struct FetchCharacterResponseParameters : IParameters
    {
        public CharacterFromDatabaseParameters? Character;

        public FetchCharacterResponseParameters(CharacterFromDatabaseParameters? character)
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