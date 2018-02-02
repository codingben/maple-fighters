using System;
using System.Diagnostics;
using CommonTools.Log;

namespace PhotonControl
{
    internal static class Utils
    {
        public static bool RunProcess(string path, string arguments = null)
        {
            try
            {
                var process = new Process
                {
                    StartInfo =
                    {
                        FileName = path
                    }
                };

                if (arguments != null)
                {
                    process.StartInfo.Arguments = arguments;
                }

                process.Start();
            }
            catch (Exception exception)
            {
                LogUtils.Log(exception.Message);
                return false;
            }

            return true;
        }
    }
}