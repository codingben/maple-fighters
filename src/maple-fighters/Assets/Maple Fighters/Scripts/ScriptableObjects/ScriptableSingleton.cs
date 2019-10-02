using Scripts.Constants;
using UnityEngine;

namespace Scripts.ScriptableObjects
{
    public class ScriptableSingleton<TObject> : ScriptableObject
        where TObject : ScriptableObject
    {
        public static TObject GetInstance()
        {
            if (instance == null)
            {
                var name = typeof(TObject).Name;
                var path = 
                    string.Format(Paths.Resources.ConfigurationsPath, name);
                instance = Resources.Load<TObject>(path);
            }

            return instance;
        }

        private static TObject instance;
    }
}