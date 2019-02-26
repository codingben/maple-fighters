using UI.Manager;
using UnityEngine;

namespace Scripts.UI
{
    public class CharacterView : UIElement, ICharacterView
    {
        public Transform Transform => transform;
    }
}