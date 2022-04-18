using Game.Physics;

namespace Game.Application.Components
{
    public interface IScenePhysicsCreator
    {
        IPhysicsWorldManager GetPhysicsWorldManager();

        IPhysicsExecutor GetPhysicsExecutor();
    }
}