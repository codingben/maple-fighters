using UnityEngine;

namespace Scripts.ScriptableObjects
{
    public class ScriptableSingleton<TObject> : ScriptableObject
        where TObject : ScriptableObject
    {
        private const string NetworkConfigurationPath = "Configurations/{0}";

        public static TObject GetInstance()
        {
            if (instance == null)
            {
                instance = Resources.Load<TObject>(
                    string.Format(NetworkConfigurationPath, typeof(TObject).Name));
            }

            return instance;
        }

        private static TObject instance;
    }
}