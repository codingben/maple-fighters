using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct ChangeSceneRequestParameters : IParameters
    {
        public Maps Map;

        public ChangeSceneRequestParameters(Maps map)
        {
            Map = map;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write((byte)Map);
        }

        public void Deserialize(BinaryReader reader)
        {
            Map = (Maps)reader.ReadByte();
        }
    }
}