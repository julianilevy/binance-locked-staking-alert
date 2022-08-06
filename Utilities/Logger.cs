using System;
using System.IO;
using System.Reflection;

namespace BinanceLockedStakingAlert
{
    public class Logger
    {
        private const string LogsFolder = "Logs";

        private enum LogType
        {
            INFO,
            WARN,
            ERROR,
            DEBUG
        }

        private static void Log(string content, LogType logType)
        {
            try
            {
                if (!Directory.Exists(ApplicationDirectory.GetFilePath(LogsFolder)))
                    ApplicationDirectory.CreateFolder(LogsFolder);

                var currentTime = DateTime.Now.ToLocalTime().ToString("HH:mm:ss");
                var millisecondsSinceStartup = "[" + MainWindow.Stopwatch.ElapsedMilliseconds + "ms" + "]";
                var logTypeString = "[" + logType + "]";

                var logName = Assembly.GetEntryAssembly().GetName().Name + "Log" + "_" + DateTime.Today.ToLocalTime().ToString("yyyy-MM-dd") + ".log";
                var extraLogData = currentTime + " " + millisecondsSinceStartup + " " + logTypeString + " ";

                using (StreamWriter streamWriter = new StreamWriter(ApplicationDirectory.GetFilePath(logName, LogsFolder), true))
                    streamWriter.WriteLine(extraLogData + content + Environment.NewLine);
            }
            catch (Exception)
            {
            }
        }

        public static void Info(string content)
        {
            Log(content, LogType.INFO);
        }

        public static void Warn(string content)
        {
            Log(content, LogType.WARN);
        }

        public static void Error(string content)
        {
            Log(content, LogType.ERROR);
        }

        public static void Error(string content, Exception e)
        {
            Log(content + ": " + e, LogType.ERROR);
        }

        public static void Debug(string content)
        {
            Log(content, LogType.DEBUG);
        }

        public static void Debug(object content)
        {
            Log(content.ToString(), LogType.DEBUG);
        }
    }
}