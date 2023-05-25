using UnityEngine;

namespace Scripts.Services.CharacterProviderApi
{
    public class CharacterDataCollection
    {
        public CharacterData[] items;

        public CharacterDataCollection(CharacterData[] items)
        {
            this.items = items;
        }

        public override string ToString()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}