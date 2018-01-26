using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct SceneObjectsRemovedEventParameters : IParameters
    {
        public int[] SceneObjectsId;

        public SceneObjectsRemovedEventParameters(int[] sceneObjectsId)
        {
            SceneObjectsId = sceneObjectsId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteInt32Array(SceneObjectsId);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneObjectsId = reader.ReadInt32Array();
        }
    }
}