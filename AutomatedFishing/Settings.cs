using AutomatedFishing.SendInput;
using System.Drawing;

namespace AutomatedFishing
{
    public class Settings
    {
        public Rectangle InteractionBox = new Rectangle(789, 711, 356, 55);

        public int RunsPerSecond
        {
            get => RunsPerSecondInternal;
            set
            {
                RunsPerSecondInternal = value;
                RunInterval = 1000 / value;
            }
        }

        public int RunsPerSecondInternal = 20;

        public int RunInterval = 50;

        public char InteractionKey
        {
            get => InteractionKeyInternal;
            set
            {
                InteractionKeyInternal = value;
                InteractionKeyScancode = (Scancodes)Enum.Parse(typeof(Scancodes), InteractionKeyInternal.ToString().ToUpper());
            }
        }

        public char InteractionKeyInternal = 'E';

        public Scancodes InteractionKeyScancode = Scancodes.E;

        public bool UsePredictionEngine = false;

        public string DestinyWindowName = "Destiny 2";
    }
}
