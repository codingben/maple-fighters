using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct SceneObjectAddedEventParameters : IParameters
    {
        public SceneObject SceneObject;

        public SceneObjectAddedEventParameters(SceneObject sceneObject)
        {
            SceneObject = sceneObject;
        }

        public void Serialize(BinaryWriter writer)
        {
            SceneObject.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObject.Deserialize(reader);
        }
    }
}