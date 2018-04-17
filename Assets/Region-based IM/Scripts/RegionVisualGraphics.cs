using System.Linq;
using TMPro;
using UnityEngine;

namespace InterestManagement.Scripts
{
    [RequireComponent(typeof(Region))]
    public class RegionVisualGraphics : MonoBehaviour
    {
        private const string GAME_OBJECT_NAME = "Region Graphics";

        [SerializeField] private int regionTextIndex = 0;
        private TextMeshPro regionText;

        private IRegion region;
        private IScene scene;

        private void Awake()
        {
            region = GetComponent<IRegion>();

            var sceneGameObject = GameObject.FindGameObjectWithTag(Scene.SCENE_TAG);
            scene = sceneGameObject.GetComponent<IScene>();
        }

        private void Update()
        {
            if (regionText != null)
            {
                regionText.text = $"Region Id: {region.Id} Entities: {region.GetAllSubscribers().Count()}";
            }
        }

        public void CreateRegionVisualGraphics()
        {
            if (scene == null)
            {
                return;
            }

            var interestAreaGraphics = Resources.Load<GameObject>(GAME_OBJECT_NAME);
            if(interestAreaGraphics == null)
            {
                return;
            }

            interestAreaGraphics.transform.localScale = new Vector3(scene.RegionSize.x, scene.RegionSize.y, interestAreaGraphics.transform.localScale.z);

            var interestAreaGraphicsGameObject = Instantiate(interestAreaGraphics, transform);
            regionText = interestAreaGraphicsGameObject.transform.GetChild(regionTextIndex).GetComponent<TextMeshPro>();
        }
    }
}