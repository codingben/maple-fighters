using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct ChangeSceneResponseParameters : IParameters
    {
        public int SceneId;

        public ChangeSceneResponseParameters(int sceneId)
        {
            SceneId = sceneId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(SceneId);
        }

        public void Deserialize(BinaryReader reader)
        {
            SceneId = reader.ReadInt32();
        }
    }
}