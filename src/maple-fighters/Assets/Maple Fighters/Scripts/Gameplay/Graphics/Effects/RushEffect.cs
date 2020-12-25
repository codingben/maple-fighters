using System.Collections;
using Scripts.Constants;
using UnityEngine;

namespace Scripts.Gameplay.Graphics
{
    public class RushEffect : MonoBehaviour
    {
        public static void Create(Vector2 position, Vector2 direction, int time)
        {
            var path =
                string.Format(Paths.Resources.Game.Graphics, "RushEffect");
            var rushEffectObject = Resources.Load<GameObject>(path);
            var rushEffectGameObject = Instantiate(
                rushEffectObject,
                position,
                Quaternion.identity);

            rushEffectGameObject.transform.localScale = direction;

            var rushEffect =
                rushEffectGameObject?.GetComponent<RushEffect>();

            rushEffect?.WaitAndDestroy(time);
        }

        public void WaitAndDestroy(int time)
        {
            StartCoroutine(WaitSomeTimeBeforeDestroy(time));
        }

        private IEnumerator WaitSomeTimeBeforeDestroy(int time)
        {
            yield return new WaitForSeconds(time);

            Destroy(gameObject);
        }
    }
}