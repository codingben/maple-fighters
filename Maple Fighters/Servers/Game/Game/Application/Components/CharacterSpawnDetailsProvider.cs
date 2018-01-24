using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;
using Game.InterestManagement;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class CharacterSpawnDetailsProvider : Component, ICharacterSpawnDetailsProvider
    {
        private readonly Dictionary<Maps, TransformDetails> charactersSpawnDetails = new Dictionary<Maps, TransformDetails>();

        public void AddCharacterSpawnDetails(Maps map, TransformDetails spawnPositionDetails)
        {
            if (!charactersSpawnDetails.ContainsKey(map))
            {
                charactersSpawnDetails.Add(map, spawnPositionDetails);
            }
            else
            {
                LogUtils.Log(MessageBuilder.Trace($"A spawn details on map {map} already exists."));
            }
        }

        public TransformDetails GetCharacterSpawnDetails(Maps map)
        {
            var hasPosition = charactersSpawnDetails.TryGetValue(map, out var spawnPosition);
            LogUtils.Assert(hasPosition, MessageBuilder.Trace($"There is no spawn details on map {map}"));
            return spawnPosition;
        }
    }
}