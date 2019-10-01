using System;
using System.Collections.Generic;
using Scripts.Coroutines;
using Scripts.Gameplay.GameEntity;
using Scripts.Gameplay.Map;
using Scripts.Gameplay.PlayerCharacter;
using Scripts.Logging;
using Scripts.Network;
using Scripts.UI.Chat;
using Scripts.UI.Focus;
using UnityEngine;

namespace Scripts.Gameplay.Creator
{
    public class GameSystemsCreator : MonoBehaviour
    {
        private void Awake()
        {
            CreateGameComponents(GetCreatorsComponents());
            CreateGameComponents(GetContainersComponents());
            CreateGameComponents(GetGUIControllersComponents());
            CreateGameComponents(GetControllersComponents());

            Destroy(gameObject);
        }

        private IEnumerable<Type> GetControllersComponents()
        {
            yield return typeof(NetworkTrafficController);
        }

        private IEnumerable<Type> GetCreatorsComponents()
        {
            yield return typeof(LogUtilsCreator);
            yield return typeof(GameTimeProviderCreator);
            yield return typeof(CharacterCreator);
            yield return typeof(EnterSceneOperationSender);
        }

        private IEnumerable<Type> GetContainersComponents()
        {
            yield return typeof(EntityContainer);
        }

        private IEnumerable<Type> GetGUIControllersComponents()
        {
            yield return typeof(FocusStateController);
            yield return typeof(ChatController);
        }

        private void CreateGameComponents(IEnumerable<Type> components)
        {
            foreach (var component in components)
            {
                var name = component.Name.MakeSpaceBetweenWords();
                var creatorGameObject = new GameObject(name);
                creatorGameObject.AddComponent(component);
            }
        }
    }
}