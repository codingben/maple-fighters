using Scripts.Containers.Service;
using UnityEngine;

namespace Scripts.Services
{
    public class Connector : MonoBehaviour
    {
        private void Start()
        {
            ServiceContainer.GameService.Connect();
        }
    }
}