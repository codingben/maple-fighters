using System.IO;
using CommonCommunicationInterfaces;
using Game.Common;

namespace Character.Server.Common
{
    public struct ChangeCharacterMapParameters : IParameters
    {
        public int UserId;
        public Maps Map;

        public ChangeCharacterMapParameters(int userId, Maps map)
        {
            UserId = userId;
            Map = map;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(UserId);
            writer.Write((byte)Map);
        }

        public void Deserialize(BinaryReader reader)
        {
            UserId = reader.ReadInt32();
            Map = (Maps)reader.ReadByte();
        }
    }
}