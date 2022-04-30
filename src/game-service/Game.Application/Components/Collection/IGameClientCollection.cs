using Fleck;

namespace Game.Application.Components
{
    public interface IGameClientCollection
    {
        void Add(IWebSocketConnection connection);
    }
}