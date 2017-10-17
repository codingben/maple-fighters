using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct LocalSceneObjectAddedEventParameters : IParameters
    {
        public SceneObject CharacterSceneObject;
        public Character Character;

        public LocalSceneObjectAddedEventParameters(SceneObject characterSceneObject, Character character)
        {
            CharacterSceneObject = characterSceneObject;
            Character = character;
        }

        public void Serialize(BinaryWriter writer)
        {
            CharacterSceneObject.Serialize(writer);
            Character.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            CharacterSceneObject.Deserialize(reader);
            Character.Deserialize(reader);
        }
    }
}