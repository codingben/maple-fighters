using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Manager
{
    public static class UIUtils
    {
        /// <summary>
        /// Creates a new element from resources.
        /// </summary>
        /// <typeparam name="TUIElement">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TUIElement"/>.
        /// </returns>
        public static TUIElement LoadAndCreateUIElement<TUIElement>()
            where TUIElement : UIElement
        {
            var name = typeof(TUIElement).Name;
            var uiGameObject =
                Utils.LoadAndCreateGameObject($"UI/{name}");
            if (uiGameObject == null)
            {
                throw new UtilsException($"The UI element {name} not found.");
            }

            uiGameObject.name = 
                uiGameObject.name.MakeSpaceBetweenWords();

            return uiGameObject.GetComponent<TUIElement>();
        }

        /// <summary>
        /// Creates an event system.
        /// </summary>
        /// <typeparam name="TEventSystem">
        /// </typeparam>
        /// <typeparam name="TStandaloneInputModule">
        /// </typeparam>
        public static void CreateEventSystem<TEventSystem, TStandaloneInputModule>()
            where TEventSystem : EventSystem
            where TStandaloneInputModule : StandaloneInputModule
        {
            if (Object.FindObjectOfType<TEventSystem>() == null)
            {
                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<TEventSystem>();
                eventSystem.AddComponent<TStandaloneInputModule>();
            }
        }
    }
}