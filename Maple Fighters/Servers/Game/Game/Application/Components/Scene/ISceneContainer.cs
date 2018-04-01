using Game.Common;

namespace Game.Application.Components
{
    public interface ISceneContainer
    {
        IGameSceneWrapper GetSceneWrapper(Maps map);
    }
}