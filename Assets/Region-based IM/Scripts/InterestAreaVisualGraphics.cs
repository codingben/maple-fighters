using UnityEngine;

namespace InterestManagement.Scripts
{
    [RequireComponent(typeof(InterestArea))]
    public class InterestAreaVisualGraphics : MonoBehaviour
    {
        public GameObject InterestAreaGraphics { get; set; }

        private const string GAME_OBJECT_NAME = "Interest Area Graphics";

        [Header("Visual Graphics")]
        [SerializeField] private bool showVisualGraphics = true;

        private IScene scene;

        private void Awake()
        {
            var sceneGameObject = GameObject.FindGameObjectWithTag(Scene.SCENE_TAG);
            scene = sceneGameObject.GetComponent<IScene>();

            if (showVisualGraphics && scene != null)
            {
                CreateInterestAreaVisualGraphics();
            }
        }

        private void CreateInterestAreaVisualGraphics()
        {
            InterestAreaGraphics = Resources.Load<GameObject>(GAME_OBJECT_NAME);
            if (InterestAreaGraphics == null)
            {
                return;
            }

            InterestAreaGraphics.transform.localScale = new Vector3(scene.RegionSize.x, scene.RegionSize.y, InterestAreaGraphics.transform.localScale.z);

            Instantiate(InterestAreaGraphics, transform);
        }
    }
}