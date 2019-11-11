using System.Collections.Generic;

namespace Scripts.UI.GameServerBrowser
{
    public struct GameServerViewCollection
    {
        private readonly IGameServerView[] collection;

        public GameServerViewCollection(int length)
        {
            collection = new IGameServerView[length];
        }

        public void Set(int index, IGameServerView gameServerView)
        {
            collection[index] = gameServerView;
        }

        public IEnumerable<IGameServerView> GetAll()
        {
            return collection;
        }
    }
}