using System.Collections.Generic;

namespace Scripts.UI.CharacterSelection
{
    public struct CharacterViewCollection
    {
        private IClickableCharacterView[] collection;

        public void Set(int index, IClickableCharacterView characterView)
        {
            if (collection == null)
            {
                collection = new IClickableCharacterView[] { null, null, null };
            }

            collection[index] = characterView;
        }
        
        public IEnumerable<IClickableCharacterView> GetAll()
        {
            if (collection == null)
            {
                collection = new IClickableCharacterView[] { null, null, null };
            }

            return collection;
        }
    }
}