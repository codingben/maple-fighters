using System.Collections;
using UnityEngine;

namespace Scripts.Gameplay.Map.Objects
{
    [RequireComponent(typeof(Animator))]
    public class Mob : MonoBehaviour
    {
        [SerializeField]
        private float hittedTime = 1f;

        private Animator animator;
        private Coroutine coroutine;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAttackedEffect()
        {
            animator.SetBool("Hitted", true);

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            coroutine = StartCoroutine(WaitAndUnsetHittedState());
        }

        private IEnumerator WaitAndUnsetHittedState()
        {
            yield return new WaitForSeconds(hittedTime);

            animator.SetBool("Hitted", false);
        }
    }
}