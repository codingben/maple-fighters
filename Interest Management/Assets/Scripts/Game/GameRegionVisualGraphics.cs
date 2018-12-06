using System.Linq;
using Assets.Scripts.Game.Utils;
using Game.InterestManagement;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameRegionVisualGraphics : MonoBehaviour
    {
        [SerializeField]
        private Color activeRegion = Color.red;

        [SerializeField]
        private Color inactiveRegion = Color.black;

        private IRegion<IGameObject> region;

        private void Update()
        {
            GraphicsUtils.DrawRectangle(
                transform.position,
                transform.localScale,
                GetColor());
        }

        public void SetRegion(IRegion<IGameObject> region)
        {
            this.region = region;
        }

        private Color GetColor()
        {
            var subscriberCount = region?.GetAllSubscribers().Count();
            return subscriberCount > 0 ? activeRegion : inactiveRegion;
        }
    }
}