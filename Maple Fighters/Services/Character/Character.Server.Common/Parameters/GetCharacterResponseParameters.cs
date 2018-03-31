using System.IO;
using CommonCommunicationInterfaces;
using Game.Common;

namespace Character.Server.Common
{
    public struct GetCharacterResponseParameters : IParameters
    {
        public CharacterParameters Character;

        public GetCharacterResponseParameters(CharacterParameters characters)
        {
            Character = characters;
        }

        public void Serialize(BinaryWriter writer)
        {
            Character.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            Character.Deserialize(reader);
        }
    }
}