using UnityEngine;

namespace InterestManagement
{
    public class Scene : MonoBehaviour
    {
        public Region[,] Regions { get; private set; }

        [SerializeField] private Vector2 sceneSize;
        [SerializeField] private Vector2 regionSize;
        [SerializeField] private GameObject regionGameObject;

        private void Awake()
        {
            var regionsX = (int) (sceneSize.x / regionSize.x);
            var regionsY = (int) (sceneSize.y / regionSize.y);

            Regions = new Region[regionsX, regionsY];

            var x = -(sceneSize.x / 2) + regionSize.x / 2;
            var y = -(sceneSize.y / 2) + regionSize.y / 2;

            for (var i = 0; i < Regions.GetLength(0); i++)
            {
                for (var j = 0; j < Regions.GetLength(1); j++)
                {
                    var region = Instantiate(regionGameObject).AddComponent<Region>();
                    region.name = $"I: {i} J: {j}";
                    region.transform.position = new Vector3(x + (i * regionSize.x), y + (j * regionSize.y));
                    region.transform.localScale = new Vector3(regionSize.x, regionSize.y);
                    region.Rectangle = new Rectangle(new Vector2(x + (i * regionSize.x), y + (j * regionSize.y)),
                        new Vector3(regionSize.x, regionSize.y));

                    Regions[i, j] = region;
                }
            }
        }
    }
}