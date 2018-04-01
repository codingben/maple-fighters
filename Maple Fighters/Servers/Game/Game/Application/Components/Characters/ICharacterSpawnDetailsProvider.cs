using Game.InterestManagement;
using Game.Common;

namespace Game.Application.Components
{
    internal interface ICharacterSpawnDetailsProvider
    {
        void AddCharacterSpawnDetails(Maps map, TransformDetails spawnPositionDetails);

        TransformDetails GetCharacterSpawnDetails(Maps map);
    }
}