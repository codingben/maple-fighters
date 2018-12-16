using Scripts.Gameplay;
using Scripts.UI.Core;
using UnityEngine;

namespace Scripts.UI
{
    public class UserInterfaceCreator : MonoBehaviour
    {
        private const string UserInterfacePath = "UI/User Interface";

        private void Awake()
        {
            if (UserInterfaceContainer.GetInstance() == null)
            {
                var userInterfaceObject = Resources.Load<GameObject>(UserInterfacePath);
                var userInterfaceGameObject = Instantiate(userInterfaceObject, Vector3.zero, Quaternion.identity);
                userInterfaceGameObject.name = userInterfaceGameObject.name.RemoveCloneFromName();
            }

            Destroy(gameObject);
        }
    }
}