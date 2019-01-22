using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class ClickableCharacterImageController : MonoBehaviour
    {
        private ClickableCharacterImage characterImage;

        private void Start()
        {
            CharactersController.GetInstance().CharacterSelected +=
                CharacterSelected;
            CharacterSelectionController.GetInstance().CharacterChosen +=
                CharacterDeselected;
            CharacterSelectionController.GetInstance().CharacterCancelled +=
                CharacterDeselected;
        }

        private void OnDestroy()
        {
            CharactersController.GetInstance().CharacterSelected -=
                CharacterSelected;
            CharacterSelectionController.GetInstance().CharacterChosen -=
                CharacterDeselected;
            CharacterSelectionController.GetInstance().CharacterCancelled -=
                CharacterDeselected;
        }

        private void CharacterSelected(ClickableCharacterImage characterImage)
        {
            this.characterImage = characterImage;

            if (characterImage != null)
            {
                characterImage.PlayAnimation(UICharacterAnimation.Walk);
            }
        }

        private void CharacterDeselected()
        {
            if (characterImage != null)
            {
                characterImage.PlayAnimation(UICharacterAnimation.Idle);
            }
        }
    }
}