using UI.Manager;
using UnityEngine;

namespace Sample.Scripts
{
    public class SampleMessageController : MonoBehaviour
    {
        private void Awake()
        {
            var sampleMessage =
                UIElementsCreator.GetInstance()
                    .Create<SampleMessage>(UILayer.Background);
            sampleMessage.Show();
        }
    }
}