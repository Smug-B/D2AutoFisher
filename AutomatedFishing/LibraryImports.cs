using AutomatedFishing.SendInput;
using System.Runtime.InteropServices;

namespace AutomatedFishing
{
    public partial class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint FindWindowA(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint GetMessageExtraInfo();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint GetForegroundWindow();
    }
}
