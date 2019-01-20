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
        public event Action<UICharacterDetails> CharacterClicked;

        [Header("Text"), SerializeField]
        private TextMeshProUGUI characterNameText;

        private UICharacterDetails uiCharacterDetails;
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

        public void SetCharacterDetails(UICharacterDetails uiCharacterDetails)
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

        public void PlayAnimation(UICharacterAnimation characterAnimation)
        {
            switch (characterAnimation)
            {
                case UICharacterAnimation.Idle:
                {
                    animator.SetBool("Walk", false);
                    break;
                }

                case UICharacterAnimation.Walk:
                {
                    animator.SetBool("Walk", true);
                    break;
                }
            }
        }

        public UICharacterDetails GetCharacterDetails()
        {
            return uiCharacterDetails;
        }
    }
}