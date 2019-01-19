using System;
using TMPro;
using UI.Manager;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UI
{
    [RequireComponent(typeof(Animator), typeof(UIFadeAnimation))]
    public class ClickableCharacterImage : UIElement, IPointerClickHandler
    {
        public event Action<UiCharacterDetails> CharacterClicked;

        [Header("Text"), SerializeField]
        private TextMeshProUGUI characterNameText;

        private UiCharacterDetails uiCharacterDetails;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            Hidden += OnHidden;
        }

        private void OnDestroy()
        {
            Hidden -= OnHidden;
        }

        private void OnHidden()
        {
            Destroy(gameObject);
        }

        public void SetCharacterDetails(UiCharacterDetails uiCharacterDetails)
        {
            this.uiCharacterDetails = uiCharacterDetails;
        }

        public void SetCharacterName(string name)
        {
            if (characterNameText != null)
            {
                characterNameText.text = name;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CharacterClicked?.Invoke(uiCharacterDetails);
        }

        public void PlayAnimation(UiCharacterAnimation characterAnimation)
        {
            switch (characterAnimation)
            {
                case UiCharacterAnimation.Idle:
                {
                    animator.SetBool("Walk", false);
                    break;
                }

                case UiCharacterAnimation.Walk:
                {
                    animator.SetBool("Walk", true);
                    break;
                }
            }
        }

        public UiCharacterDetails GetCharacterDetails()
        {
            return uiCharacterDetails;
        }
    }
}