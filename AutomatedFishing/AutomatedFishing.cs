using AutomatedFishing.Inputs;
using AutomatedFishing.Utility;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace AutomatedFishing
{
    public class AutomatedFishing : IDisposable
    {
        public static InteractionBoxDetector.ModelInput InteractionPredictor { get; }

        public CancellationTokenSource FishingToken { get; }

        public int FishingCounter { get; private set; }

        private bool HasInteracted;

        static AutomatedFishing()
        {
            InteractionPredictor = new InteractionBoxDetector.ModelInput();
        }

        public AutomatedFishing()
        {
            FishingToken = new CancellationTokenSource();
            FishingCounter = 0;
            HasInteracted = false;
            Task.Run(DetectAvailableInteraction, FishingToken.Token);
        }

        public void DetectAvailableInteraction()
        {
            try
            {
                while (true)
                {
                    FishingToken.Token.ThrowIfCancellationRequested();

                    nint destinyHandle = Program.FindWindowA(null, Program.Settings.DestinyWindowName);
                    if (destinyHandle == 0)
                    {
                       throw new Exception("Destiny 2 window not found.");
                    }

                    if (destinyHandle == Program.GetForegroundWindow())
                    {
                        using Bitmap rawCaptureBitmap = ScreenUtility.GetScreenAsBitmap();
                        if (Program.Settings.UsePredictionEngine)
                        {
                            using Bitmap captureBitmap = rawCaptureBitmap.Clone(Program.Settings.InteractionBox, PixelFormat.Format32bppArgb);
                            InteractionPredictor.ImageSource = ScreenUtility.BitmapToMLImage(captureBitmap);
                            InteractionBoxDetector.ModelOutput result = InteractionBoxDetector.Predict(InteractionPredictor);
                            if (result.Prediction == "Interact")
                            {
                                Reeling.InteractDown(Program.Settings.InteractionKeyScancode);
                                HasInteracted = true;
                            }
                            else if (HasInteracted)
                            {
                                Reeling.InteractUp(Program.Settings.InteractionKeyScancode);
                            }
                        }
                        else
                        {
                            BitmapData captureBitmapData = rawCaptureBitmap.LockBits(Program.Settings.InteractionBox, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
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
                                Reeling.InteractDown(Program.Settings.InteractionKeyScancode);
                                HasInteracted = true;
                            }
                            else if (HasInteracted)
                            {
                                Reeling.InteractUp(Program.Settings.InteractionKeyScancode);
                            }
                        }
                    }

                    Thread.CurrentThread.Join(Program.Settings.RunsPerSecond);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Running automatic fishing routine failed with error.");
                Console.WriteLine(exception.Message);
            }
        }

        public void StopFishing() => FishingToken?.Cancel();

        public void Dispose()
        {
            FishingToken?.Cancel();
            FishingToken?.Dispose();
            InteractionPredictor.ImageSource?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
