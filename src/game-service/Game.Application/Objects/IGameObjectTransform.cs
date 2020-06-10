using InterestManagement;

namespace Game.Application.Objects
{
    public interface IGameObjectTransform : ITransform
    {
        byte Direction { get; }

        void SetDirection(byte direction);
    }
}