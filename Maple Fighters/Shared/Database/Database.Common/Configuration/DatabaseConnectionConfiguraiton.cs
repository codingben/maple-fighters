namespace Database.Common.Configuration
{
    public static class DatabaseConnectionConfiguraiton
    {
        public static string GetConnectionString()
        {
            const string HOST = "127.0.0.1";
            const int PORT = 3306;
            const string USERNAME = "root";
            const string PASSWORD = "Abenzuk91790";
            return string.Format($"User ID={USERNAME};Password={PASSWORD};Host={HOST};Port={PORT};KeepAlive=30;Initial Catalog=\'fighters\';");
        }
    }
}