using System.Collections.Generic;

namespace Scripts.UI.CharacterSelection
{
    public struct CharacterViewCollection
    {
        private readonly IClickableCharacterView[] collection;

        public CharacterViewCollection(IClickableCharacterView[] views)
        {
            collection = views;
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