using Newtonsoft.Json;

namespace AutomatedFishing
{
    public partial class Program
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static Settings Settings { get; internal set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public static AutomatedFishing AutomatedFishing;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new Exception("Automated Fishing needs to be started through D2 Auto Fisher!");
                }

                Settings? deserializedData = (Settings?)JsonConvert.DeserializeObject(args[0], typeof(Settings)) ?? throw new Exception("Could not deserialize arguments to proper type.");
                Settings = deserializedData;
                Console.WriteLine("Successfully loaded settings from D2 Auto Fisher!");
                Console.WriteLine("Now running Automated Fisher! Write anything to kill this console.");
                AutomatedFishing = new AutomatedFishing();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}