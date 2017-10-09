using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EnterWorldResponseParameters : IParameters
    {
        public GameObject PlayerGameObject;
        public Character Character;
        public bool HasCharacter;

        public EnterWorldResponseParameters(GameObject? playerGameObject, Character? character, bool hasCharacter)
        {
            PlayerGameObject = playerGameObject.GetValueOrDefault();
            Character = character.GetValueOrDefault();
            HasCharacter = hasCharacter;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(HasCharacter);
            
            if (HasCharacter)
            {
                PlayerGameObject.Serialize(writer);
                Character.Serialize(writer);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            HasCharacter = reader.ReadBoolean();

            if (HasCharacter)
            {
                PlayerGameObject.Deserialize(reader);
                Character.Deserialize(reader);
            }
        }
    }
}