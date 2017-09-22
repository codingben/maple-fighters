using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct GameObjectAddedEventParameters : IParameters
    {
        public GameObject GameObject;

        public GameObjectAddedEventParameters(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public void Serialize(BinaryWriter writer)
        {
            GameObject.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameObject.Deserialize(reader);
        }
    }
}