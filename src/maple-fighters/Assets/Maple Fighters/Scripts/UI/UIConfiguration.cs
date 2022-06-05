using System.Linq;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(
        fileName = "UIConfiguration",
        menuName = "UI Manager/UIConfiguration",
        order = 0)]
    public class UIConfiguration : ScriptableObject
    {
        public static UIConfiguration GetInstance()
        {
            if (instance == null)
            {
                var name = typeof(UIConfiguration).Name;
                var path = string.Format(UIConstants.UIResources, name);
                instance = Resources.Load<UIConfiguration>(path);

                if (instance == null)
                {
                    throw new UIConfigurationNotFoundException();
                }
            }

            return instance;
        }

        private static UIConfiguration instance;

        public UICanvasConfig[] CanvasConfig;

        public UICanvasConfig GetCanvasConfig(UICanvasType canvasType)
        {
            return CanvasConfig.FirstOrDefault((x) => x.CanvasType == canvasType);
        }
    }
}