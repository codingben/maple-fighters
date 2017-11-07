using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class CharacterSpawnPositionDetailsProvider : Component<IServerEntity>, ICharacterSpawnPositionDetailsProvider
    {
        private readonly Dictionary<Maps, SpawnPositionDetails> spawnPositionsDetails = new Dictionary<Maps, SpawnPositionDetails>();

        public void AddSpawnPositionDetails(Maps map, SpawnPositionDetails spawnPositionDetails)
        {
            if (spawnPositionsDetails.ContainsKey(map))
            {
                LogUtils.Log(MessageBuilder.Trace($"A spawn position on map {map} already exists."));
                return;
            }

            spawnPositionsDetails.Add(map, spawnPositionDetails);
        }

        public SpawnPositionDetails GetSpawnPositionDetails(Maps map)
        {
            var hasPosition = spawnPositionsDetails.TryGetValue(map, out var spawnPosition);
            LogUtils.Assert(hasPosition, MessageBuilder.Trace($"There is no spawn position on map {map}"));
            return spawnPosition;
        }
    }
}