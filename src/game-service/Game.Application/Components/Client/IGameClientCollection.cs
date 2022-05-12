namespace Game.Application.Components
{
    public interface IGameClientCollection
    {
        void Add(IGameClient gameClient);

        void Remove(int id);

        bool TryGet(int id, out IGameClient gameClient);
    }
}