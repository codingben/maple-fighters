using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct SceneObjectRemovedEventParameters : IParameters
    {
        public int SceneObjectId;

        public SceneObjectRemovedEventParameters(int sceneObjectId)
        {
            SceneObjectId = sceneObjectId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SceneObjectId);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjectId = reader.ReadInt32();
        }
    }
}