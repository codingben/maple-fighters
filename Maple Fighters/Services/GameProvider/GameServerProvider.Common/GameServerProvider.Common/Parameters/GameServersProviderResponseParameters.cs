using System.IO;
using CommonCommunicationInterfaces;
using CommunicationHelper;

namespace GameServerProvider.Client.Common
{
    public struct GameServersProviderResponseParameters : IParameters
    {
        public GameServerInformationParameters[] GameServerInformations;

        public GameServersProviderResponseParameters(GameServerInformationParameters[] gameServerInformations)
        {
            GameServerInformations = gameServerInformations;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.WriteArray(GameServerInformations);
        }

        public void Deserialize(BinaryReader reader)
        {
            GameServerInformations = reader.ReadArray<GameServerInformationParameters>();
        }
    }
}