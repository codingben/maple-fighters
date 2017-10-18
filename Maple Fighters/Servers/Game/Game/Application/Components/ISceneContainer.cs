using ComponentModel.Common;
using MathematicsHelper;
using Shared.Game.Common;

namespace Game.Application.Components
{
    public interface ISceneContainer : IExposableComponent
    {
        void AddScene(byte map, Vector2 sceneSize, Vector2 regionSize);

        IGameSceneWrapper GetSceneWrapper(Maps map);
    }
}