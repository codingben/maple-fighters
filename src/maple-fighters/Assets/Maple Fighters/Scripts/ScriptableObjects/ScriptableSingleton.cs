using UnityEngine;

namespace Scripts.ScriptableObjects
{
    public class ScriptableSingleton<TObject> : ScriptableObject
        where TObject : ScriptableObject
    {
        private const string ConfigPath = "Configurations/{0}";

        public static TObject GetInstance()
        {
            if (instance == null)
            {
                var name = typeof(TObject).Name;
                var path = string.Format(ConfigPath, name);
                instance = Resources.Load<TObject>(path);
            }

            return instance;
        }

        private static TObject instance;
    }
}