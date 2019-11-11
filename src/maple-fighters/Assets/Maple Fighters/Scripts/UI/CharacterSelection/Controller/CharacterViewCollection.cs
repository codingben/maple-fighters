using System.Collections.Generic;

namespace Scripts.UI.CharacterSelection
{
    public struct CharacterViewCollection
    {
        private readonly IClickableCharacterView[] collection;

        public CharacterViewCollection(IClickableCharacterView[] view = null)
        {
            if (view == null)
            {
                collection = new IClickableCharacterView[] { null, null, null };
            }

            collection = view;
        }

        public void Set(int index, IClickableCharacterView characterView)
        {
            collection[index] = characterView;
        }
        
        public IEnumerable<IClickableCharacterView> GetAll()
        {
            return collection;
        }
    }
}