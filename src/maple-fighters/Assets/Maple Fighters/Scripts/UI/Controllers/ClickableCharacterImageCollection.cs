namespace Scripts.UI.Controllers
{
    public struct ClickableCharacterImageCollection : ICharacterImageCollection
    {
        private IClickableCharacterView[] clickableCharacterViews;
        
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
            if (clickableCharacterViews == null)
            {
                clickableCharacterViews =
                    new IClickableCharacterView[] { null, null, null };
            }

            return clickableCharacterViews;
        }
    }
}