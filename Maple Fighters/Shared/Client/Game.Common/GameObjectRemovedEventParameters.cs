using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct GameObjectRemovedEventParameters : IParameters
    {
        public int GameObjectId;

        public GameObjectRemovedEventParameters(int gameObjectId)
        {
            GameObjectId = gameObjectId;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(GameObjectId);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObjectId = reader.ReadInt32();
        }
    }
}