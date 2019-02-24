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
        public event Action<UICharacterClass> CharacterClicked;

        public UICharacterClass CharacterClass
        {
            set
            {
                characterClass = value;
            }
        }

        public string CharacterName
        {
            set
            {
                if (characterNameText != null)
                {
                    characterNameText.text = value;
                }
            }
        }

        [Header("Text"), SerializeField]
        private TextMeshProUGUI characterNameText;

        private UICharacterClass characterClass;
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
        
        public void OnPointerClick(PointerEventData eventData)
        {
            CharacterClicked?.Invoke(characterClass);
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
    }
}