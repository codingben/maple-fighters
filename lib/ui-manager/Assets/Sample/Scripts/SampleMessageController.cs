using UI;
using UnityEngine;

namespace Sample.Scripts
{
    /// <summary>
    /// The creator of the <see cref="SampleMessage"/>.
    /// </summary>
    public class SampleMessageController : MonoBehaviour
    {
        private void Awake()
        {
            var sampleMessage = UICreator
                .GetInstance()
                .Create<SampleMessage>(UICanvasLayer.Background);
            if (sampleMessage != null)
            {
                sampleMessage.Show();
            }
        }
    }
}