using System;
using InterestManagement;

namespace Game.Application.Objects
{
    public class InterestManagementLogger : ILogger
    {
        public static ILogger GetLogger()
        {
            if (logger == null)
            {
                logger = new InterestManagementLogger();
            }

            return logger;
        }

        private static InterestManagementLogger logger;

        public void Info(string message) => Console.WriteLine(message);
    }
}