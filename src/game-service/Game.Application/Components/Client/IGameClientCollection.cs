using System.Collections.Generic;

namespace Game.Application.Components
{
    public interface IGameClientCollection : IEnumerable<IGameClient>
    {
        bool Add(IGameClient gameClient);

        void Remove(int id);

        bool TryGet(int id, out IGameClient gameClient);

        int Count();
    }
}