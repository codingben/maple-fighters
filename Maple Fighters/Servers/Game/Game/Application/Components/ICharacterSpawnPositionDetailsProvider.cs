using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.Components
{
    internal interface ICharacterSpawnPositionDetailsProvider : IExposableComponent
    {
        void AddSpawnPositionDetails(Maps map, SpawnPositionDetails spawnPositionDetails);

        SpawnPositionDetails GetSpawnPositionDetails(Maps map);
    }
}