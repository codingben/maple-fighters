using CommonTools.Log;
using UnityEngine;

namespace Scripts.Utils
{
    public class LogUtilsCreator : MonoBehaviour
    {
        private void Awake()
        {
            LogUtils.Logger = new Logger();

            Destroy(gameObject);
        }
    }
}