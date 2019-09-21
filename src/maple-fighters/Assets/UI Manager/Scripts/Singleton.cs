using UnityEngine;

namespace UI.Manager
{
    public class Singleton<TObject> : MonoBehaviour
        where TObject : Singleton<TObject>
    {
        public static TObject GetInstance()
        {
            if (instance == null)
            {
                var name = typeof(TObject).Name;
                var component = typeof(TObject);
                instance =
                    new GameObject(name, component).GetComponent<TObject>();
            }

            return instance;
        }

        private static TObject instance;
    }
}