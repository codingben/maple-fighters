using System;

namespace Scripts.UI.MapScene
{
    public interface IMessageView
    {
        Action TimeUp { get; set; }

        string Text { set; }

        float Seconds { set; }
    }
}