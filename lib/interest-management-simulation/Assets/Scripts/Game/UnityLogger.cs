using UnityEngine;

namespace InterestManagement
{
    public class UnityLogger : ILogger
    {
        public void Info(string message)
        {
            Debug.Log(message);
        }
    }
}