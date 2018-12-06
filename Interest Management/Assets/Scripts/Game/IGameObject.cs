using Game.InterestManagement;

namespace Assets.Scripts.Game
{
    public interface IGameObject : ISceneObject
    {
        void SetGraphics(bool active);
    }
}