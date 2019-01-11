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
            var uiGameObject =
                Utils.LoadAndCreateGameObject($"UI/{typeof(TUIElement).Name}");
            if (uiGameObject != null)
            {
                uiGameObject.name = 
                    uiGameObject.name.MakeSpaceBetweenWords();

                return uiGameObject.GetComponent<TUIElement>();
            }

            throw new UtilsException(
                $"The UI element {typeof(TUIElement).Name} not found.");
        }
    }
}