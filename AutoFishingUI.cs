using D2AutoFisher.Utility;
using Newtonsoft.Json;
using SmugBase.Logging;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;

namespace D2AutoFisher
{
    public partial class AutoFishingUI : Form
    {
        public static Process? AutomatedFishing => Program.AutomatedFishing;

        public AutoFishingUI()
        {
            InitializeComponent();
        }

        private void AutoFishingUI_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(Program.ConfigPath))
                {
                    Program.Logger.Log("Failed to find pre-existing configuration. Ignore this if you are a first time user.", LogType.Warning);
                    return;
                }

                string deserializationText = File.ReadAllText(Program.ConfigPath);
                if (deserializationText == null)
                {
                    throw new Exception("Deserialized results were null.");
                }

                Settings? deserializedData = (Settings?)JsonConvert.DeserializeObject(deserializationText, typeof(Settings));
                if (deserializedData == null)
                {
                    throw new Exception("Could not deserialize FishConfigs.json to proper type.");
                }

                Program.Settings = deserializedData;
                RPSInput.Value = Program.Settings.RunsPerSecondInternal;
                KeyInput.Text = Program.Settings.InteractionKey.ToString();
                PredictionEngineCheckbox.Checked = Program.Settings.UsePredictionEngine;
                Program.Logger.Log("Successfully loaded setting configurations.");
            }
            catch (Exception exception)
            {
                Program.Logger.Log("Failed to load config files with error.", LogType.Error);
                Program.Logger.Log(exception, LogType.Error);
                MessageBox.Show("[Warning] Failed to load config files with error: " + exception.Message);
            }
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.Settings.InteractionBox == Rectangle.Empty)
                {
                    string errorMessage = "Failed to find calibration box. "
                    + "Please follow the instructions and press 'Calibrate' when relevant before running auto-fisher!";
                    throw new Exception(errorMessage);
                }

                if (AutomatedFishing != null)
                {
                    Program.Logger.Log("Found existing automatic fishing routine.", LogType.Warning);
                    AutomatedFishing.Kill();
                    AutomatedFishing.Dispose();
                    Program.Logger.Log("Terminated pre-existing automatic fishing routine.");
                }

                Program.Logger.Log("Started automatic fishing routine.");
                Program.AutomatedFishing = Process.Start("AutomatedFishing/AutomatedFishing.exe", new string[] { JsonConvert.SerializeObject(Program.Settings), JsonConvert.SerializeObject(SystemInformation.VirtualScreen) });
                MessageBox.Show("Sucessfully started new automatic fishing routine in a seperate console window, this window will automatically close to preserve resources.");
                Environment.Exit(0);
            }
            catch (Exception exception)
            {
                string errorMessage = "Running automatic fishing routine failed with error: " + exception.Message;
                Program.Logger.Log(errorMessage, LogType.Error);
                MessageBox.Show(errorMessage);
            }
        }

        private void CalibrateButton_Click(object sender, EventArgs e)
        {
            try
            {
                Program.Logger.Log("Beginning calibration routine with a three second delay.");

                Thread.Sleep(3000);

                if (Program.FindWindowA(null, Program.Settings.DestinyWindowName) == 0)
                {
                    throw new Exception("Failed to find Destiny 2 window.");
                }

                ScreenUtility.GetScreenAsBitmap().Save(@"Calibration_Input.jpg", ImageFormat.Jpeg);
                Program.Logger.Log("Recorded screen data for calibration data.");

                List<Bitmap> captures = ScreenUtility.GetScreensAsBitmap();
                int x = 0;
                for (int i = 0; i < captures.Count; i++)
                {
                    using Bitmap captureBitmap = captures[i];
                    int screenWidth = captureBitmap.Width;
                    int screenHeight = captureBitmap.Height;

                    if (i != 0)
                    {
                        x += screenWidth;
                    }

                    captureBitmap.Save(@"Calibration_Input_" + i + ".jpg", ImageFormat.Jpeg);
                    Program.Logger.Log("Recorded screen data for calibration data.");

                    InteractionBoxLocator.ModelInput calibrationInput = new InteractionBoxLocator.ModelInput()
                    {
                        ImageSource = ScreenUtility.BitmapToMLImage(captureBitmap),
                    };
                    Program.Logger.Log("Initialized indicator detector input.");

                    InteractionBoxLocator.ModelOutput calibrationOutput = InteractionBoxLocator.Predict(calibrationInput);
                    int detectedLabels = calibrationOutput.Scores.Length;
                    if (detectedLabels != 0)
                    {
                        Program.Logger.Log("Found relevant fishing indicators.", LogType.Info);

                        using Graphics captureBitmapGraphics = Graphics.FromImage(captureBitmap);
                        Program.Logger.Log("Preparing calibration output images for human verification.", LogType.Info);

                        float xScale = (float)screenWidth / InteractionBoxLocator.ModelInput.ImageTransformX;
                        float yScale = (float)screenHeight / InteractionBoxLocator.ModelInput.ImageTransformY;
                        InteractionBoxLocator.ModelOutput.BoundingBox box = calibrationOutput.BoundingBoxes.First(box => box.Label == "Interaction Box");
                        Color markingColor = box.Label switch
                        {
                            "Interaction Box" => Color.IndianRed,
                            _ => Color.CornflowerBlue,
                        };
                        using Pen markingPen = new Pen(markingColor, 4);
                        int width = (int)((box.Right - box.Left) * xScale);
                        int height = (int)((box.Bottom - box.Top) * yScale);
                        Program.Settings.InteractionBox = new Rectangle((int)(box.Left * xScale), (int)(box.Top * yScale), (int)((box.Right - box.Left) * xScale), (int)((box.Bottom - box.Top) * yScale));
                        Program.Settings.InteractionBox.X += x;
                        captureBitmapGraphics.DrawRectangle(markingPen, Program.Settings.InteractionBox);
                        Program.Logger.Log("Found " + box.Label + " at (" + box.Left + ", " + box.Top + ") [Scaling Factor: (" + xScale + ", " + yScale + ")] "
                        + "with confidence " + box.Score + "%", LogType.Info);
                        captureBitmap.Save(@"Calibration_Output_" + i + ".jpg", ImageFormat.Jpeg);
                        Program.Logger.Log("Recorded calibration output images.");
                        goto endCalibration;
                    }
                }

                throw new Exception("Failed to find relevant fishing indicators.");

                endCalibration:
                string calibrationSerialized = JsonConvert.SerializeObject(Program.Settings);
                using FileStream jsonStream = new FileStream(Program.ConfigPath, FileMode.Create);
                jsonStream.Write(Encoding.ASCII.GetBytes(calibrationSerialized));
                Program.Logger.Log("Saved calibration data as " + Program.ConfigPath);

                MessageBox.Show("Calibration succeeded and saved to " + Program.ConfigPath + "; please confirm validity through inspecting 'Calibration_Output.jpg'."
                + "\nApplication will be automatically restarted for a clean reset of resources.");
            }
            catch (Exception exception)
            {
                string errorMessage = "Calibration failed with error: " + exception.Message;
                Program.Logger.Log(errorMessage, LogType.Error);
                MessageBox.Show(errorMessage);
            }
            finally
            {
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private void SaveSettingsButton_Click(object sender, EventArgs e)
        {
            try
            {
                Program.Settings.RunsPerSecond = (int)RPSInput.Value;

                char interactionKey = KeyInput.Text.ToUpper()[0];
                if (interactionKey < 'A' || interactionKey > 'Z')
                {
                    string errorMessage = "Failed to update interaction key. New key was not an alphabetical letter.";
                    Program.Logger.Log(errorMessage, LogType.Error);
                    MessageBox.Show(errorMessage);
                }
                else
                {
                    Program.Settings.InteractionKey = KeyInput.Text[0];
                }

                Program.Settings.UsePredictionEngine = PredictionEngineCheckbox.Checked;

                string calibrationSerialized = JsonConvert.SerializeObject(Program.Settings);
                using FileStream jsonStream = new FileStream(Program.ConfigPath, FileMode.Create);
                jsonStream.Write(Encoding.ASCII.GetBytes(calibrationSerialized));
                Program.Logger.Log("Saved settings as " + Program.ConfigPath);

                MessageBox.Show("Settings successfully saved as " + Program.ConfigPath + " (located in the same directory as the executable)."
                + "\nIt's not advised that you change anything directly with the configs unless you know what you're doing."
                + "\nMore details on the specifics of every setting can be found on the GitHub wiki.");
            }
            catch (Exception exception)
            {
                string errorMessage = "Saving settings failed with error: " + exception.Message;
                Program.Logger.Log(errorMessage, LogType.Error);
                MessageBox.Show(errorMessage);
            }
        }

        private void GitHubLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                LinkLabel linkLabel = (LinkLabel)sender;
                linkLabel.LinkVisited = true;
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "https://github.com/SmugBlanco/D2AutoFisher",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception exception)
            {
                string errorMessage = "Visiting GitHub failed with error: " + exception.Message;
                Program.Logger.Log(errorMessage, LogType.Error);
                MessageBox.Show(errorMessage);
            }
        }
    }
}
