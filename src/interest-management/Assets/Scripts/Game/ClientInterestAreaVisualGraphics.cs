using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    public class ClientInterestAreaVisualGraphics : MonoBehaviour
    {
        [SerializeField]
        private Color color = Color.green;

        private void Update()
        {
            GraphicsUtils.DrawRectangle(
                transform.position, transform.localScale, color);
        }
    }
}