using Scripts.Containers.Entity;

namespace Scripts.Containers
{
    public static class GameContainers
    {
        public static IEntityContainer EntityContainer
        {
            get
            {
                if (_entityContainer == null)
                {
                    _entityContainer = new EntityContainer();
                }

                return _entityContainer;
            }
        }

        private static IEntityContainer _entityContainer;
    }
}