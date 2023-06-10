using SmugBase.Loading;
using SmugBase.Logging;
using SmugBase.Utility;
using System.Diagnostics;

namespace D2AutoFisher
{
    public partial class Program
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static Logger Logger { get; internal set; }

        public static Settings Settings { get; internal set; }

        public static AutoFishingUI AutoFishingUI { get; internal set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public static Process? AutomatedFishing { get; internal set; }

        public static string ConfigPath => @"FishingConfigs.json";

        [STAThread]
        static void Main()
        {
            Logger = new Logger("Main.log", FileUtility.GetDirectory("D2 Auto Fisher", "Logs"));
            LoadingHandler.ImplementLoading(Logger);
            Settings = new Settings();
            ApplicationConfiguration.Initialize();
            AutoFishingUI = new AutoFishingUI();
            Application.Run(AutoFishingUI);
        }
    }
}