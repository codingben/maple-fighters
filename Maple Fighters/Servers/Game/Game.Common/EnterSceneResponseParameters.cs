using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EnterSceneResponseParameters : IParameters
    {
        public SceneObject SceneObject;
        public Character Character;

        public EnterSceneResponseParameters(SceneObject sceneObject, Character character)
        {
            SceneObject = sceneObject;
            Character = character;
        }

        public void Serialize(BinaryWriter writer)
        {
            SceneObject.Serialize(writer);
            Character.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObject.Deserialize(reader);
            Character.Deserialize(reader);
        }
    }
}