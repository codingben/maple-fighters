using ComponentModel.Common;
using Game.Common;

namespace Game.Application.Components
{
    public interface ISceneContainer : IExposableComponent
    {
        IGameSceneWrapper GetSceneWrapper(Maps map);
    }
}