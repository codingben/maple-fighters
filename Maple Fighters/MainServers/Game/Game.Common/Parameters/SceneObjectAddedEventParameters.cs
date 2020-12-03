using System.IO;
using CommonCommunicationInterfaces;

namespace Game.Common
{
    public struct SceneObjectAddedEventParameters : IParameters
    {
        public SceneObjectParameters SceneObject;

        public SceneObjectAddedEventParameters(SceneObjectParameters sceneObject)
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