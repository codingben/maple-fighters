using System.Linq;
using TMPro;
using UnityEngine;

namespace InterestManagement.Scripts
{
    [RequireComponent(typeof(Region))]
    public class RegionVisualGraphics : MonoBehaviour
    {
        private const string GAME_OBJECT_NAME = "Region Graphics";

        [Header("Visual Graphics")]
        [SerializeField] private bool showVisualGraphics = true;
        [SerializeField] private int regionTextIndex = 0;

        private TextMeshPro regionText;

        private IRegion region;
        private IScene scene;

        private void Awake()
        {
            region = GetComponent<IRegion>();

            var sceneGameObject = GameObject.FindGameObjectWithTag(Scene.SCENE_TAG);
            scene = sceneGameObject.GetComponent<IScene>();

            if (showVisualGraphics && scene != null)
            {
                CreateRegionVisualGraphics();
            }
        }

        private void Update()
        {
            if (regionText != null)
            {
                regionText.text = $"Region Id: {region.Id} Entities: {region.GetAllSubscribers().Count()}";
            }
        }

        private void CreateRegionVisualGraphics()
        {
            var interestAreaGraphics = Resources.Load<GameObject>(GAME_OBJECT_NAME);
            if(interestAreaGraphics == null)
            {
                return;
            }

            interestAreaGraphics.transform.localScale = new Vector3(scene.RegionSize.x, scene.RegionSize.y, interestAreaGraphics.transform.localScale.z);
            regionText = interestAreaGraphics.transform.GetChild(regionTextIndex).GetComponent<TextMeshPro>();

            Instantiate(interestAreaGraphics, transform);
        }
    }
}