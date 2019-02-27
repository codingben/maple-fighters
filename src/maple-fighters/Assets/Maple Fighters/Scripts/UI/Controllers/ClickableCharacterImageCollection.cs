namespace Scripts.UI.Controllers
{
    public struct ClickableCharacterImageCollection
    {
        private IClickableCharacterView[] clickableCharacterViews;
        
        public void SetCharacterView(
            UICharacterIndex uiCharacterIndex,
            IClickableCharacterView clickableCharacterView)
        {
            if (uiCharacterIndex != UICharacterIndex.Zero)
            {
                GetClickableCharacterViews()[(int)uiCharacterIndex] =
                    clickableCharacterView;
            }
        }

        public IClickableCharacterView GetCharacterView(
            UICharacterIndex uiCharacterIndex)
        {
            return uiCharacterIndex == UICharacterIndex.Zero
                       ? null
                       : GetClickableCharacterViews()[(int)uiCharacterIndex];
        }
        
        public IClickableCharacterView[] GetClickableCharacterViews()
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