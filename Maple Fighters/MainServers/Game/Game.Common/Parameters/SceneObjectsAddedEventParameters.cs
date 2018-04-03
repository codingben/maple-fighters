using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Game.Common
{
    public struct SceneObjectsAddedEventParameters : IParameters
    {
        public SceneObjectParameters[] SceneObjects;

        public SceneObjectsAddedEventParameters(SceneObjectParameters[] sceneObjects)
        {
            SceneObjects = sceneObjects;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(SceneObjects);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjects = reader.ReadArray<SceneObjectParameters>();
        }
    }
}