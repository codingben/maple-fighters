using UnityEngine;

namespace Scripts.UI.Controllers
{
    public class ClickableCharacterImageController : MonoBehaviour
    {
        private ClickableCharacterImage characterImage;

        private void Awake()
        {
            // TODO: Use event bus system
            var characterViewController =
                FindObjectOfType<CharacterViewController>();
            if (characterViewController != null)
            {
                characterViewController.CharacterSelected += CharacterSelected;
            }

            var characterSelectionController =
                FindObjectOfType<CharacterSelectionController>();
            if (characterSelectionController != null)
            {
                characterSelectionController.CharacterChosen +=
                    CharacterDeselected;
                characterSelectionController.CharacterCancelled +=
                    CharacterDeselected;
            }
        }

        private void OnDestroy()
        {
            // TODO: Use event bus system
            var characterViewController =
                FindObjectOfType<CharacterViewController>();
            if (characterViewController != null)
            {
                characterViewController.CharacterSelected -= CharacterSelected;
            }

            var characterSelectionController =
                FindObjectOfType<CharacterSelectionController>();
            if (characterSelectionController != null)
            {
                characterSelectionController.CharacterChosen -=
                    CharacterDeselected;
                characterSelectionController.CharacterCancelled -=
                    CharacterDeselected;
            }
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