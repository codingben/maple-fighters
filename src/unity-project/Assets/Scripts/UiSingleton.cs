using UnityEngine;

namespace UserInterface
{
    public class UiSingleton<TObject> : MonoBehaviour
        where TObject : UiSingleton<TObject>
    {
        public static TObject GetInstance()
        {
            if (instance == null)
            {
                instance = UiUtils.CreateUiElement<TObject>();
            }

            return instance;
        }

        private static TObject instance;
    }
}