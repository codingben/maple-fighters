using Assets.Scripts.Game.Utils;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class ClientInterestAreaVisualGraphics : MonoBehaviour
    {
        [SerializeField]
        private Color color = Color.green;

        private void Update()
        {
            GraphicsUtils.DrawRectangle(
                transform.position,
                transform.localScale,
                color);
        }
    }
}