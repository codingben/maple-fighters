namespace Scripts.Services
{
    // TODO: Don't use user data as a static object
    public static class UserData
    {
        public static class CharacterData
        {
            public static int Type = -1;

            public static string Name;
        }

        // TODO: JWT

        public static int Id;

        public static string GameServerUrl;
    }
}