using System.IO;
using CommonCommunicationInterfaces;

namespace Shared.Game.Common
{
    public struct ChangeSceneResponseParameters : IParameters
    {
        public Maps Map;

        public ChangeSceneResponseParameters(Maps map)
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