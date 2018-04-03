namespace UserProfile.Server.Common
{
    internal interface IUserIdToPeerIdConverter
    {
        void Add(int userId, int peerId);
        void Remove(int userId);

        int? Get(int userId);
    }
}