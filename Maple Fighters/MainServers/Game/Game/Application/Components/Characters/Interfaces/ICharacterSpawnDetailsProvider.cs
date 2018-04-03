using Game.Common;
using InterestManagement;

namespace Game.Application.Components.Interfaces
{
    internal interface ICharacterSpawnDetailsProvider
    {
        void AddCharacterSpawnDetails(Maps map, TransformDetails spawnPositionDetails);

        TransformDetails GetCharacterSpawnDetails(Maps map);
    }
}