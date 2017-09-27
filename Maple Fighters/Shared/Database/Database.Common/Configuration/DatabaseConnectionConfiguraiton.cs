namespace Database.Common.Configuration
{
    public static class DatabaseConnectionConfiguraiton
    {
        public static string GetConnectionString()
        {
            const string HOST = "localhost";
            const int PORT = 5432;
            const string USERNAME = "postgres";
            const string PASSWORD = "ben91790";

            return string.Format($"User ID={USERNAME};Password={PASSWORD};Host={HOST};Port={PORT};KeepAlive=30;");
        }
    }
}