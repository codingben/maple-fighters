using Game.Application.Objects;
using InterestManagement;

namespace Game.Application.Components
{
    public interface IGameScene : IScene<IGameObject>
    {
        PlayerSpawnData PlayerSpawnData { get; set; }
    }
}