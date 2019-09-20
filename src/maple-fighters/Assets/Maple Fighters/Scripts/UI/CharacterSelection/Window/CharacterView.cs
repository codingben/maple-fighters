using UI.Manager;
using UnityEngine;

namespace Scripts.UI.CharacterSelection
{
    public class CharacterView : UIElement, ICharacterView
    {
        public Transform Transform => transform;
    }
}