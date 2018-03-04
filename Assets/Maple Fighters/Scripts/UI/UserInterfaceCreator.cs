using Scripts.UI.Core;
using UnityEngine;

namespace Scripts.UI
{
    public class UserInterfaceCreator : MonoBehaviour
    {
        private const string USER_INTERFACE_FROM_RESOURCES = "UI/User Interface";

        private void Awake()
        {
            if (UserInterfaceContainer.Instance == null)
            {
                var userInterfaceObject = Resources.Load<GameObject>(USER_INTERFACE_FROM_RESOURCES);
                var userInterfaceGameObject = Instantiate(userInterfaceObject, Vector3.zero, Quaternion.identity);
                userInterfaceGameObject.name = userInterfaceGameObject.name.RemoveCloneFromName();
            }

            Destroy(gameObject);
        }
    }
}