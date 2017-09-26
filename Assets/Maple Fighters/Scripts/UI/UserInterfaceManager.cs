using System;
using System.Collections.Generic;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.UI
{
    public class UserInterfaceManager : DontDestroyOnLoad<UserInterfaceManager>
    {
        private readonly List<IUserInterface> userInterfaces = new List<IUserInterface>();

        public IUserInterface Add<T>(T type)
            where T : IUserInterface
        {
            var userInterfaceName = typeof(T).Name;

            var userInterfaceObject = Resources.FindObjectsOfTypeAll(typeof(T));
            if (userInterfaceObject == null)
            {
                throw new Exception($"Could not find an user interface with name {userInterfaceName}");
            }

            if (userInterfaceObject.Length > 1)
            {
                throw new Exception($"There is more than one similar objects of type {userInterfaceName}");
            }

            var userInterfaceGameObject = userInterfaceObject[0] as GameObject;
            if (userInterfaceGameObject == null)
            {
                throw new Exception($"Something went wrong, could not convert a type {userInterfaceName} to game object.");
            }

            Instantiate(userInterfaceGameObject, userInterfaceGameObject.transform.position, Quaternion.identity, gameObject.transform)
                .transform.SetAsFirstSibling();

            userInterfaces.Add(type);
            return type;
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
    }
}