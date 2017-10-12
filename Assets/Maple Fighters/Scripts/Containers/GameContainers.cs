namespace Scripts.Containers
{
    public static class GameContainers
    {
        public static IGameObjectsContainer GameObjectsContainer
        {
            get
            {
                if (_gameObjectsContainer == null)
                {
                    _gameObjectsContainer = new GameObjectsContainer();
                }
                return _gameObjectsContainer;
            }
        }

        private static IGameObjectsContainer _gameObjectsContainer;
    }
}