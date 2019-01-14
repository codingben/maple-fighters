namespace Scripts.UI.Controllers
{
    public class ClickableCharacterImageController
    {
        private ClickableCharacterImage characterImage;

        public void SetCharacterImage(ClickableCharacterImage characterImage)
        {
            this.characterImage = characterImage;
        }

        public void CharacterSelected()
        {
            if (characterImage != null)
            {
                characterImage.PlayAnimation(UiCharacterAnimation.Walk);
            }
        }

        public void CharacterDeselected()
        {
            if (characterImage != null)
            {
                characterImage.PlayAnimation(UiCharacterAnimation.Idle);
            }
        }
    }
}