using ComponentModel.Common;
using Shared.Game.Common;

namespace Game.Application.Components
{
    public interface ISceneContainer : IExposableComponent
    {
        IGameSceneWrapper GetSceneWrapper(Maps map);
    }
}