using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.UI
{
    public class UserInterfaceContainer : DontDestroyOnLoad<UserInterfaceContainer>
    {
        private const string UI_RESOURCES_PATH = "UI/{0}";
        private readonly List<IUserInterface> userInterfaces = new List<IUserInterface>();

        [SerializeField] private Transform parent;

        public T AddOnly<T>(T type)
            where T : IUserInterface
        {
            if (Get<T>() != null)
            {
                var userInterfaceName = typeof(T).Name;
                throw new Exception($"An interface with name {userInterfaceName} already exists in a container.");
            }

            userInterfaces.Add(type);
            return type;
        }

        public T Add<T>()
            where T : IUserInterface
        {
            var userInterfaceName = typeof(T).Name;

            var userInterfaceObject = Resources.Load(string.Format(UI_RESOURCES_PATH, userInterfaceName)) as GameObject;
            if (userInterfaceObject == null)
            {
                throw new Exception($"Could not find an user interface with name {userInterfaceName}.");
            }

            var userInterface = Instantiate(userInterfaceObject, Vector3.zero, Quaternion.identity, parent)
                .GetComponent<T>();
            userInterface.GameObject.transform.SetAsFirstSibling();
            userInterface.GameObject.GetComponent<RectTransform>().anchoredPosition = userInterfaceObject.transform.position;

            userInterfaces.Add(userInterface);
            return userInterface;
        }

        public void Remove<T>(T type)
            where T : IUserInterface
        {
            if (!userInterfaces.Contains(type))
            {
                var userInterfaceName = typeof(T).Name;
                throw new Exception($"Could not remove an user interface with name {userInterfaceName} because it does not exist.");
            }

            Destroy(type.GameObject);

            userInterfaces.Remove(type);
        }

        public T Get<T>()
            where T : IUserInterface
        {
            var userInterface = userInterfaces.OfType<T>().FirstOrDefault();
            return userInterface;
        }
    }
}