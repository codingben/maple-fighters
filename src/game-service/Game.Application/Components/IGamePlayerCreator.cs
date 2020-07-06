using Game.Application.Objects;

namespace Game.Application.Components
{
    public interface IGamePlayerCreator
    {
        IGameObject Create();
    }
}