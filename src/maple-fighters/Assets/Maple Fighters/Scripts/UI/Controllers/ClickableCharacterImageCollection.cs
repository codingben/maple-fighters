namespace Scripts.UI.Controllers
{
    public struct ClickableCharacterImageCollection
    {
        private IClickableCharacterView[] clickableCharacterViews;

        public ClickableCharacterImageCollection(IClickableCharacterView[] clickableCharacterViews)
        {
            this.clickableCharacterViews = clickableCharacterViews;
        }

        public void Set(UICharacterIndex uiCharacterIndex, IClickableCharacterView clickableCharacterView)
        {
            if (uiCharacterIndex != UICharacterIndex.Zero)
            {
                GetAll()[(int)uiCharacterIndex] = clickableCharacterView;
            }
        }

        public IClickableCharacterView Get(UICharacterIndex uiCharacterIndex)
        {
            return uiCharacterIndex == UICharacterIndex.Zero
                       ? null
                       : GetAll()[(int)uiCharacterIndex];
        }
        
        public IClickableCharacterView[] GetAll()
        {
            return clickableCharacterViews;
        }
    }
}