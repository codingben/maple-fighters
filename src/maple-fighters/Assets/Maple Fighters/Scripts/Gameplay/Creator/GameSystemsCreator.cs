using System;
using System.Collections.Generic;
using Network.Utils;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Map;
using Scripts.Gameplay.PlayerCharacter;
using Scripts.Services.Chat;
using Scripts.Services.Game;
using Scripts.UI.Chat;
using Scripts.UI.Focus;
using Scripts.World.Dummy;
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
            CreateGameComponents(GetSetterComponents());
            CreateGameComponents(GetServicesComponents());

            Destroy(gameObject);
        }

        private IEnumerable<Type> GetCreatorsComponents()
        {
            yield return typeof(CharacterCreator);
            yield return typeof(EnterSceneOperationSender);
        }

        private IEnumerable<Type> GetContainersComponents()
        {
            yield return typeof(EntityContainer);
            yield return typeof(DummyPortalContainer);
        }

        private IEnumerable<Type> GetGUIControllersComponents()
        {
            yield return typeof(FocusStateController);
            yield return typeof(ChatController);
        }

        private IEnumerable<Type> GetSetterComponents()
        {
            yield return typeof(LoggerSetter);
            yield return typeof(DefaultTimeProviderSetter);
        }

        private IEnumerable<Type> GetServicesComponents()
        {
            if (FindObjectOfType<GameService>() == null)
            {
                yield return typeof(GameService);
                yield return typeof(DummyGameServiceConnector);
            }

            if (FindObjectOfType<ChatService>() == null)
            {
                yield return typeof(ChatService);
            }
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