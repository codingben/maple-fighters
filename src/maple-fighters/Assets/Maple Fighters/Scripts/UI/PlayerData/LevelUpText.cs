using UI;
using UnityEngine;

namespace Scripts.UI.PlayerData
{
    [RequireComponent(typeof(UIFadeAnimation))]
    public class LevelUpText : UIElement
    {
        [SerializeField]
        private float moveSpeed;

        private void Start()
        {
            SubscribeToUIFadeAnimation();
        }

        private void OnDestroy()
        {
            UnsubscribeFromUIFadeAnimation();
        }

        private void Update()
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }

        private void SubscribeToUIFadeAnimation()
        {
            var uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeOutCompleted += OnFadeOutCompleted;
        }

        private void UnsubscribeFromUIFadeAnimation()
        {
            var uiFadeAnimation = GetComponent<UIFadeAnimation>();
            uiFadeAnimation.FadeOutCompleted -= OnFadeOutCompleted;
        }

        private void OnFadeOutCompleted()
        {
            Destroy(gameObject);
        }
    }
}