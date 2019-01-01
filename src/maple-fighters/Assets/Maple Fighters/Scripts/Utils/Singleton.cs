namespace Scripts.Utils
{
    public class Singleton<T>
        where T : class, new()
    {
        public static T GetInstance()
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }

        private static T instance;
    }
}