using Scripts.Constants;
using UnityEngine;

namespace ScriptableObjects.Utils
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
                    string.Format(Paths.Resources.Configurations, name);
                instance = Resources.Load<TObject>(path);
            }

            return instance;
        }

        private static TObject instance;
    }
}