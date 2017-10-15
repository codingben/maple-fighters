using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct LocalGameObjectAddedEventParameters : IParameters
    {
        public GameObject CharacterGameObject;
        public Character Character;

        public LocalGameObjectAddedEventParameters(GameObject characterGameObject, Character character)
        {
            CharacterGameObject = characterGameObject;
            Character = character;
        }

        public void Serialize(BinaryWriter writer)
        {
            CharacterGameObject.Serialize(writer);
            Character.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharacterGameObject.Deserialize(reader);
            Character.Deserialize(reader);
        }
    }
}