namespace Scripts.UI.Controllers
{
    public struct ClickableCharacterImageCollection
    {
        private ClickableCharacterImage[] clickableCharacterImages;
        
        public void SetCharacterImage(
            UICharacterIndex uiCharacterIndex,
            ClickableCharacterImage clickableCharacterImage)
        {
            if (uiCharacterIndex != UICharacterIndex.Zero)
            {
                GetClickableCharacterImages()[(int)uiCharacterIndex] =
                    clickableCharacterImage;
            }
        }

        public ClickableCharacterImage GetCharacterImage(
            UICharacterIndex uiCharacterIndex)
        {
            return uiCharacterIndex == UICharacterIndex.Zero
                       ? null
                       : GetClickableCharacterImages()[(int)uiCharacterIndex];
        }
        
        public ClickableCharacterImage[] GetClickableCharacterImages()
        {
            if (clickableCharacterImages == null)
            {
                clickableCharacterImages =
                    new ClickableCharacterImage[] { null, null, null };
            }

            return clickableCharacterImages;
        }
    }
}