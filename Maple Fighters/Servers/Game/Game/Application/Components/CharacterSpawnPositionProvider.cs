using System.Collections.Generic;
using CommonTools.Log;
using ComponentModel.Common;
using MathematicsHelper;
using ServerApplication.Common.ApplicationBase;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal class CharacterSpawnPositionProvider : Component<IServerEntity>, ICharacterSpawnPositionProvider
    {
        private readonly Dictionary<Maps, Vector2> spawnPositions = new Dictionary<Maps, Vector2>();

        public void AddPosition(Maps map, Vector2 position)
        {
            if (spawnPositions.ContainsKey(map))
            {
                LogUtils.Log(MessageBuilder.Trace($"A spawn position on map {map} already exists."));
                return;
            }

            spawnPositions.Add(map, position);
        }

        public Vector2 GetPosition(Maps map)
        {
            var hasPosition = spawnPositions.TryGetValue(map, out var position);
            LogUtils.Assert(hasPosition, MessageBuilder.Trace($"There is no spawn position on map {map}"));
            return position;
        }
    }
}