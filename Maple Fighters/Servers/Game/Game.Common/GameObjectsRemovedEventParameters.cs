using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct GameObjectsRemovedEventParameters : IParameters
    {
        public int[] GameObjectsId;

        public GameObjectsRemovedEventParameters(int[] gameObjectsId)
        {
            GameObjectsId = gameObjectsId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteInt32Array(GameObjectsId);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObjectsId = reader.ReadInt32Array();
        }
    }
}