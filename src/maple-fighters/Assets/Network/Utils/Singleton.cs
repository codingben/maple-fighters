using UnityEngine;

namespace Network.Utils
{
    public class Singleton<TObject> : MonoBehaviour
        where TObject : Singleton<TObject>
    {
        public static TObject GetInstance()
        {
            if (instance == null)
            {
                var type = typeof(TObject);
                var name = type.Name;
                var gameObject = new GameObject(name, type);

                instance = gameObject.GetComponent<TObject>();
            }

            return instance;
        }

        private static TObject instance;
    }
}