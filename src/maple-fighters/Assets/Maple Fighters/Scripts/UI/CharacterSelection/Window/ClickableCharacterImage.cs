using System;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.UI.CharacterSelection
{
    [RequireComponent(typeof(Animator), typeof(UIFadeAnimation))]
    public class ClickableCharacterImage : UIElement,
                                           IPointerClickHandler,
                                           IClickableCharacterView
    {
        public event Action<UICharacterIndex, bool> CharacterClicked;

        public int Id { get; set; }

        public string CharacterName
        {
            get
            {
                var name = string.Empty;

                if (characterNameText != null)
                {
                    name = characterNameText.text;
                }

                return name;
            }

            set
            {
                if (characterNameText != null)
                {
                    characterNameText.text = value;
                }
            }
        }

        public int CharacterLevel
        {
            get
            {
                return characterLevel;
            }

            set
            {
                characterLevel = value;

                if (characterLevelText != null)
                {
                    characterLevelText.text = $"Lvl. {characterLevel}";
                }
            }
        }

        public float CharacterExperience
        {
            get
            {
                return characterExperience;
            }

            set
            {
                characterExperience = value;

                if (characterExperienceText != null)
                {
                    characterExperienceText.text = $"({characterExperience}%)";
                }
            }
        }

        public UICharacterIndex CharacterIndex { get; set; }

        public UICharacterClass CharacterClass { get; set; }

        public GameObject GameObject => gameObject;

        [Header("Text"), SerializeField]
        private Text characterNameText;

        [Header("Text"), SerializeField]
        private Text characterLevelText;

        [Header("Text"), SerializeField]
        private Text characterExperienceText;

        private Animator animator;

        private int characterLevel;
        private float characterExperience;

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
            var hasCharacter = CharacterClass != UICharacterClass.Sample;

            CharacterClicked?.Invoke(CharacterIndex, hasCharacter);
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