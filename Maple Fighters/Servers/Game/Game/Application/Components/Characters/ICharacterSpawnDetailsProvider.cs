using ComponentModel.Common;
using Game.InterestManagement;
using Game.Common;

namespace Game.Application.Components
{
    internal interface ICharacterSpawnDetailsProvider : IExposableComponent
    {
        void AddCharacterSpawnDetails(Maps map, TransformDetails spawnPositionDetails);

        TransformDetails GetCharacterSpawnDetails(Maps map);
    }
}