using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Gameplay;
using Scripts.Utils;
using UnityEngine;

namespace Scripts.UI.Core
{
    public enum ViewType
    {
        Background,
        Foreground
    }

    public enum Index
    {
        First,
        Last
    }

    public class UserInterfaceContainer : DontDestroyOnLoad<UserInterfaceContainer>
    {
        private const string UI_RESOURCES_PATH = "UI/{0}";

        [Header("Views")]
        [SerializeField] private Transform background;
        [SerializeField] private Transform foreground;

        private readonly List<IUserInterface> userInterfaces = new List<IUserInterface>();

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

        public T Add<T>(ViewType viewType = ViewType.Foreground, Index index = Index.First, Transform parent = null)
            where T : IUserInterface
        {
            var userInterfaceName = typeof(T).Name;
            var userInterfaceObject = Resources.Load(string.Format(UI_RESOURCES_PATH, userInterfaceName)) as GameObject;
            if (userInterfaceObject == null)
            {
                throw new Exception($"Could not find an user interface with name {userInterfaceName}.");
            }

            var parentView = viewType == ViewType.Foreground ? foreground : background;
            var userInterface = Instantiate(userInterfaceObject, Vector3.zero, Quaternion.identity, parentView)
                .GetComponent<T>();
            userInterface.GameObject.name = userInterface.GameObject.name.RemoveCloneFromName();
            userInterface.GameObject.name = userInterface.GameObject.name.MakeSpaceBetweenWords();

            if (parent != null)
            {
                userInterface.GameObject.transform.SetParent(parent, true);
            }

            if (index == Index.First)
            {
                userInterface.GameObject.transform.SetAsFirstSibling();
            }
            else
            {
                userInterface.GameObject.transform.SetAsLastSibling();
            }

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