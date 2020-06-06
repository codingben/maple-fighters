using Common.ComponentModel;

namespace Game.Application.Objects.Components
{
    [ComponentSettings(ExposedState.Exposable)]
    public class GameObjectGetter : ComponentBase, IGameObjectGetter
    {
        private readonly IGameObject gameObject;

        public GameObjectGetter(IGameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public IGameObject GetGameObject()
        {
            return gameObject;
        }
    }
}