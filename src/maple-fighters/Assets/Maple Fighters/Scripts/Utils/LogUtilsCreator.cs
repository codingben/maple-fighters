using CommonTools.Log;
using UnityEngine;

namespace Scripts.Network
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