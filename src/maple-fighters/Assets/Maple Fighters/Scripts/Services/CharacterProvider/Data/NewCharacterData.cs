using System;

namespace Scripts.Services.CharacterProviderApi
{
    [Serializable]
    public class NewCharacterData
    {
        public int userid;

        public string charactername;

        public int index;

        public int classindex;
    }
}