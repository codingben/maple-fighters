using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct SceneObjectsAddedEventParameters : IParameters
    {
        public SceneObject[] SceneObjects;

        public SceneObjectsAddedEventParameters(SceneObject[] sceneObjects)
        {
            SceneObjects = sceneObjects;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(SceneObjects);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjects = reader.ReadArray<SceneObject>();
        }
    }
}