using System;

namespace Scripts.Services.CharacterProviderApi
{
    [Serializable]
    public struct CharacterData
    {
        public int id;

        public string userid;

        public string charactername;

        public int characterlevel;

        public float characterexperience;

        public int index;

        public int classindex;
    }
}