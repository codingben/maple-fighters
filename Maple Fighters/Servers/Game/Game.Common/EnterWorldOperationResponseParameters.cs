using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct EnterWorldOperationResponseParameters : IParameters
    {
        public GameObject PlayerGameObject;

        public EnterWorldOperationResponseParameters(GameObject playerGameObject)
        {
            PlayerGameObject = playerGameObject;
        }

        public void Serialize(BinaryWriter writer)
        {
            PlayerGameObject.Serialize(writer);
        }

        public void Deserialize(BinaryReader reader)
        {
            PlayerGameObject.Deserialize(reader);
        }
    }
}