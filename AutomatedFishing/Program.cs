using AutomatedFishing.Inputs;
using AutomatedFishing.Utility;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AutomatedFishing
{
    public partial class Program
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public static Settings Settings { get; internal set; }

        public static Rectangle VirtualScreen { get; internal set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private static bool HasInteracted = false;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 2)
                {
                    throw new Exception("Automated Fishing needs to be started through D2 Auto Fisher!");
                }

                Settings = (Settings?)JsonConvert.DeserializeObject(args[0], typeof(Settings)) ?? throw new Exception("Could not deserialize arguments to proper type.");
                VirtualScreen = (Rectangle)JsonConvert.DeserializeObject(args[1], typeof(Rectangle));
                if (VirtualScreen.IsEmpty)
                {
                    throw new Exception("Could not deserialize arguments to proper type.");
                }

                Console.WriteLine("Successfully loaded settings from D2 Auto Fisher!");
                Console.WriteLine("Now running Automated Fisher! Write anything to kill this console.");
                Task.Run(() =>
                {
                    try
                    {
                        InteractionBoxDetector.ModelInput InteractionPredictor = new InteractionBoxDetector.ModelInput();

                        while (true)
                        {
                            nint destinyHandle = FindWindowA(null, Settings.DestinyWindowName);
                            if (destinyHandle == 0)
                            {
                                throw new Exception("Destiny 2 window not found.");
                            }

                            if (destinyHandle == GetForegroundWindow())
                            {
                                using Bitmap rawCaptureBitmap = ScreenUtility.GetScreenAsBitmap();
                                if (Settings.UsePredictionEngine)
                                {
                                    using Bitmap captureBitmap = rawCaptureBitmap.Clone(Settings.InteractionBox, PixelFormat.Format32bppArgb);
                                    InteractionPredictor.ImageSource = ScreenUtility.BitmapToMLImage(captureBitmap);
                                    InteractionBoxDetector.ModelOutput result = InteractionBoxDetector.Predict(InteractionPredictor);
                                    if (result.Prediction == "Interact")
                                    {
                                        Reeling.InteractDown(Settings.InteractionKeyScancode);
                                        HasInteracted = true;
                                    }
                                    else if (HasInteracted)
                                    {
                                        Reeling.InteractUp(Settings.InteractionKeyScancode);
                                    }
                                }
                                else
                                {
                                    BitmapData captureBitmapData = rawCaptureBitmap.LockBits(Settings.InteractionBox, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                                    byte[] Pixels = new byte[captureBitmapData.Width * captureBitmapData.Height * 4];
                                    nint pixelPointer = captureBitmapData.Scan0;

                                    Marshal.Copy(pixelPointer, Pixels, 0, Pixels.Length);
                                    bool foundWhite = false;
                                    for (int i = 0; i < Pixels.Length; i += 4)
                                    {
                                        if (Pixels[i] == 255 && Pixels[i + 1] == 255 && Pixels[i + 2] == 255)
                                        {
                                            foundWhite = true;
                                            break;
                                        }
                                    }

                                    if (foundWhite)
                                    {
                                        Reeling.InteractDown(Settings.InteractionKeyScancode);
                                        HasInteracted = true;
                                    }
                                    else if (HasInteracted)
                                    {
                                        Reeling.InteractUp(Settings.InteractionKeyScancode);
                                    }
                                }
                            }

                            Thread.CurrentThread.Join(Settings.RunsPerSecond);
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine("Running automatic fishing routine failed with error.");
                        Console.WriteLine(exception.Message);
                    }
                });
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