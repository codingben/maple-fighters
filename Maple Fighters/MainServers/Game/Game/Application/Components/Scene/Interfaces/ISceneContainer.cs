using Game.Common;

namespace Game.Application.Components.Interfaces
{
    public interface ISceneContainer
    {
        IGameSceneWrapper GetSceneWrapper(Maps map);
    }
}