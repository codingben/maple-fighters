using CommonTools.Log;
using JsonConfig;

namespace Database.Common.Configuration
{
    public static class DatabaseConnectionConfiguraiton
    {
        public static string GetConnectionString()
        {
            LogUtils.Assert(Config.Global.Database, MessageBuilder.Trace("Could not find a configuration for the database."));

            var ip = (string)Config.Global.Database.Ip;
            var port = (int)Config.Global.Database.Port;
            var userName = (string)Config.Global.Database.Username;
            var password = (string)Config.Global.Database.Password;
            var table = (string)Config.Global.Database.Table;
            return string.Format($"Host={ip};Port={port};User ID={userName};Password={password};KeepAlive=30;Initial Catalog=\'{table}\';");
        }
    }
}