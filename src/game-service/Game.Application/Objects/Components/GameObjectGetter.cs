using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    public class GameObjectGetter : ComponentBase, IGameObjectGetter
    {
        private readonly IGameObject gameObject;

        public GameObjectGetter(IGameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public IGameObject Get()
        {
            return gameObject;
        }
    }
}