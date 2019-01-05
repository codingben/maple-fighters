using UnityEngine;

namespace UserInterface
{
    public static class UiUtils
    {
        public static TUiElement CreateUiElement<TUiElement>()
            where TUiElement : class // UiElement
        {
            const string UiElementsPath = "UI/{0}";

            TUiElement uiElement;

            var userInterfaceObject =
                Resources.Load(string.Format(UiElementsPath, typeof(TUiElement).Name))
                    as GameObject;
            if (userInterfaceObject != null)
            {
                var userInterfaceGameObject = Object.Instantiate(
                    userInterfaceObject,
                    Vector3.zero,
                    Quaternion.identity);

                uiElement =
                    userInterfaceGameObject.GetComponent<TUiElement>();
            }
            else
            {
                throw new UiException(
                    $"The UI element {typeof(TUiElement).Name} not found.");
            }

            return uiElement;
        }
    }
}