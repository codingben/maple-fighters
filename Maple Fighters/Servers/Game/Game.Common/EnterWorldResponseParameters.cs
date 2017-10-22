using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EnterWorldResponseParameters : IParameters
    {
        public SceneObject CharacterSceneObject;
        public Character Character;

        public EnterWorldResponseParameters(SceneObject characterSceneObject, Character character)
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