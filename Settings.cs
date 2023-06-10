using D2AutoFisher.SendInput;

namespace D2AutoFisher
{
    public class Settings
    {
        public Rectangle InteractionBox = Rectangle.Empty;

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
