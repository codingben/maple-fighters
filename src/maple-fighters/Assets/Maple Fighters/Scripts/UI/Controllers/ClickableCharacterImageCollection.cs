using System.Collections.Generic;

namespace Scripts.UI.Controllers
{
    public struct ClickableCharacterImageCollection
    {
        private ClickableCharacterImage[] clickableCharacterImages;
        
        public void SetCharacterImage(UiCharacterIndex uiCharacterIndex, ClickableCharacterImage clickableCharacterImage)
        {
            if (clickableCharacterImages == null)
            {
                clickableCharacterImages = new ClickableCharacterImage[] { null, null, null };
            }

            if (uiCharacterIndex != UiCharacterIndex.Zero)
            {
                clickableCharacterImages[(int)uiCharacterIndex] = clickableCharacterImage;
            }
        }

        public ClickableCharacterImage GetCharacterImage(UiCharacterIndex uiCharacterIndex)
        {
            if (clickableCharacterImages == null)
            {
                clickableCharacterImages = new ClickableCharacterImage[] { null, null, null };
            }

            return uiCharacterIndex == UiCharacterIndex.Zero
                       ? null
                       : clickableCharacterImages[(int)uiCharacterIndex];
        }

        public IEnumerable<ClickableCharacterImage> GetCharacterImages()
        {
            if (clickableCharacterImages == null)
            {
                clickableCharacterImages = new ClickableCharacterImage[] { null, null, null };
            }

            foreach (var characterImage in clickableCharacterImages)
            {
                yield return characterImage;
            }
        }
    }
}