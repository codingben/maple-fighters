using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace Shared.Game.Common
{
    public struct GameObjectsAddedEventParameters : IParameters
    {
        public GameObject[] GameObjects;

        public GameObjectsAddedEventParameters(GameObject[] entity)
        {
            GameObjects = entity;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(GameObjects);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObjects = reader.ReadArray<GameObject>();
        }
    }
}