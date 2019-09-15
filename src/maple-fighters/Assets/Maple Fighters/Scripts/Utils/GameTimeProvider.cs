using CommonTools.Coroutines;
using UnityEngine;

namespace Scripts.Utils
{
    public class GameTimeProvider : ITimeProvider
    {
        public float GetCurrentTime()
        {
            return Time.time;
        }
    }
}