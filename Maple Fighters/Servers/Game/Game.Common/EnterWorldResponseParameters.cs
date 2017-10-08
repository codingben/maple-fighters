using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EnterWorldResponseParameters : IParameters
    {
        public GameObject PlayerGameObject;
        public Character Character;

        public EnterWorldResponseParameters(GameObject playerGameObject, Character character)
        {
            PlayerGameObject = playerGameObject;
            Character = character;
        }

        public void Serialize(BinaryWriter writer)
        {
            PlayerGameObject.Serialize(writer);
            Character.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            PlayerGameObject.Deserialize(reader);
            Character.Deserialize(reader);
        }
    }
}