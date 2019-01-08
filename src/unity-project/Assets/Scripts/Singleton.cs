using UnityEngine;

namespace UserInterface
{
    public class Singleton<TObject> : MonoBehaviour
        where TObject : Singleton<TObject>
    {
        public static TObject GetInstance()
        {
            if (instance == null)
            {
                instance = (new GameObject(typeof(TObject).Name, typeof(TObject)))
                    .GetComponent<TObject>();
            }

            return instance;
        }

        private static TObject instance;
    }
}