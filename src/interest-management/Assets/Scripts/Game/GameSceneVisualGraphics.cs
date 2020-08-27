using System.Collections;
using InterestManagement;
using UnityEngine;

namespace Game.InterestManagement.Simulation
{
    [RequireComponent(typeof(GameScene))]
    public partial class GameSceneVisualGraphics : MonoBehaviour
    {
        private const string RegionObject = "Region";

        [Header("The higher value the smaller region will be.")]
        [SerializeField]
        private Vector2 size = new Vector2(0.1f, 0.1f);

        private IScene<IGameObject> scene;

        private void Awake()
        {
            var gameScene = GetComponent<GameScene>();
            scene = gameScene.GetScene();
        }

        private void Start()
        {
            StartCoroutine(CreateRegions());
        }

        private IEnumerator CreateRegions()
        {
            var index = 0;
            var regionObject = Resources.Load<GameObject>(RegionObject);
            var regions = scene.MatrixRegion.GetAllRegions();

            foreach (var region in regions)
            {
                var gameRegion = new GameRegion(index, region);
                var regionGameObject = Instantiate(
                    regionObject,
                    gameRegion.Position,
                    Quaternion.identity,
                    transform);
                regionGameObject.transform.localScale = gameRegion.Size - size;
                regionGameObject.name =
                    $"Index: {index} (X: {gameRegion.Size.x} Y: {gameRegion.Size.y})";

                var gameRegionVisualGraphics =
                    regionGameObject.GetComponent<GameRegionVisualGraphics>();
                if (gameRegionVisualGraphics != null)
                {
                    gameRegionVisualGraphics.SetRegion(region);
                }

                index++;
                yield return null;
            }
        }
    }
}