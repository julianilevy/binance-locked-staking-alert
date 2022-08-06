using System;
using System.IO;
using System.Windows.Threading;
using Newtonsoft.Json;

namespace BinanceLockedStakingAlert
{
    public static class Utilities
    {
        public static T GetConfig<T>(string configFile)
        {
            try
            {
                using (var streamReader = new StreamReader(configFile))
                {
                    var config = JsonConvert.DeserializeObject<T>(streamReader.ReadToEnd());

                    Logger.Info($"{configFile} successfully loaded");

                    return config;
                }
            }
            catch (Exception e)
            {
                Logger.Error($"An error occurred while loading {configFile}", e);

                return default(T);
            }
        }

        // https://stackoverflow.com/questions/10458118/wait-one-second-in-running-program
        public static void DelayAction(int millisecond, Action action)
        {
            var timer = new DispatcherTimer();
            timer.Tick += delegate
            {
                action.Invoke();
                timer.Stop();
            };

            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
        }
    }
}