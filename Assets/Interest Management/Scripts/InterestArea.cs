using UnityEngine;

namespace InterestManagement
{
    public class InterestArea : MonoBehaviour
    {
        public Scene Scene;

        private Rectangle rectangle;
        private Vector3 lastPosition;

        private const int ENTITY_ID = 1;

        private void Awake()
        {
            rectangle = new Rectangle(Vector2.zero, transform.localScale);
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, lastPosition) < 1)
            {
                return;
            }

            rectangle.SetPosition(new Vector2(transform.position.x, transform.position.y));

            foreach (var region in Scene.Regions)
            {
                if (!Rectangle.Intersect(region.Rectangle, rectangle).Equals(Rectangle.EMPTY))
                {
                    Debug.LogWarning(region.name);

                    if (region.GetEntity(ENTITY_ID))
                    {
                        continue;
                    }

                    region.AddEntity(ENTITY_ID);
                }
                else
                {
                    if (region.GetEntity(ENTITY_ID))
                    {
                        region.RemoveEntity(ENTITY_ID);
                    }
                }
            }

            lastPosition = transform.position;
        }
    }
}