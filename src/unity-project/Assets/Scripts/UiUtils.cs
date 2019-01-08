namespace UserInterface
{
    public static class UiUtils
    {
        /// <summary>
        /// Creates a new element from resources.
        /// </summary>
        /// <typeparam name="TUiElement">
        /// </typeparam>
        /// <returns>
        /// The <see cref="TUiElement"/>.
        /// </returns>
        public static TUiElement LoadAndCreateUiElement<TUiElement>()
            where TUiElement : UiElement
        {
            var uiGameObject =
                Utils.LoadAndCreateGameObject($"UI/{typeof(TUiElement).Name}");
            if (uiGameObject != null)
            {
                uiGameObject.name = 
                    uiGameObject.name.MakeSpaceBetweenWords();

                return uiGameObject.GetComponent<TUiElement>();
            }

            throw new UtilsException(
                $"The UI element {typeof(TUiElement).Name} not found.");
        }
    }
}