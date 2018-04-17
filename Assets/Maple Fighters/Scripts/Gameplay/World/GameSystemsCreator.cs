using System;
using System.Collections.Generic;
using Game.Common;
using Scripts.Containers;
using Scripts.Gameplay.Actors;
using Scripts.Services;
using Scripts.UI;
using Scripts.UI.Controllers;
using Scripts.World;
using UnityEngine;

namespace Scripts.Gameplay
{
    public class GameSystemsCreator : MonoBehaviour
    {
        private void Awake()
        {
            CreateGameSystems(GetCreatorsComponents());
            CreateGameSystems(GetContainersComponents());
            CreateGameSystems(GetGUIControllersComponents());
            CreateGameSystems(GetConnectionProviderComponents());

            Destroy(gameObject);
        }

        private IEnumerable<Type> GetCreatorsComponents()
        {
            yield return typeof(LogUtilsCreator);
            yield return typeof(GameTimeProviderCreator);
            yield return typeof(UserInterfaceCreator);
            yield return typeof(CharacterCreator);

            if (ServiceContainer.GameService.ServiceConnectionHandler.IsConnected())
            {
                yield return typeof(EnterSceneInvoker);
            }
            else
            {
                ServiceContainer.GameService.SetPeerLogic<GameScenePeerLogic, GameOperations, GameEvents>();
            }
        }

        private IEnumerable<Type> GetContainersComponents()
        {
            yield return typeof(SceneObjectsContainer);
        }

        private IEnumerable<Type> GetGUIControllersComponents()
        {
            yield return typeof(FocusController);
            yield return typeof(ChatController);
        }

        private IEnumerable<Type> GetConnectionProviderComponents()
        {
            yield return typeof(ChatConnectionProvider);
        }

        private void CreateGameSystems(IEnumerable<Type> components)
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