using System;
using System.Collections.Generic;
using Scripts.Gameplay.Entity;
using Scripts.Gameplay.Map;
using Scripts.Gameplay.Map.Dummy;
using Scripts.Gameplay.Player;
using Scripts.Services.Game;
using Scripts.UI.Chat;
using Scripts.UI.Focus;
using UI.Manager;
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
            CreateGameComponents(GetApiComponents());

            Destroy(gameObject);
        }

        private IEnumerable<Type> GetCreatorsComponents()
        {
            yield return typeof(CharacterCreator);
            yield return typeof(EnterMapMessageSender);
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

        private IEnumerable<Type> GetApiComponents()
        {
            if (FindObjectOfType<GameApi>() == null)
            {
                yield return typeof(GameApi);
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