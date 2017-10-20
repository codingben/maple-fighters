using Game.InterestManagement;
using MathematicsHelper;

namespace Game.Application.Components
{
    public interface IGameSceneWrapper
    {
        void AddSceneObject(string name, Vector2 position);

        IScene GetScene();
    }
}