using Game.InterestManagement;

namespace Game.Application.Components
{
    public interface IGameSceneWrapper : ISceneEntity
    {
        IScene GetScene();

        void AddScenePhysicsData();
    }
}