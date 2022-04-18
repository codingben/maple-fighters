using Game.Application.Objects;
using InterestManagement;

namespace Game.Application.Components
{
    public interface ISceneRegionCreator
    {
        IMatrixRegion<IGameObject> GetRegion();
    }
}