using CommonTools.Coroutines;
using UnityEngine;

namespace Scripts.Coroutines
{
    public class GameTimeProvider : ITimeProvider
    {
        public float GetCurrentTime()
        {
            return Time.time;
        }
    }
}