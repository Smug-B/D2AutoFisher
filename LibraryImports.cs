using D2AutoFisher.SendInput;
using D2AutoFisher.Windows;
using System.Runtime.InteropServices;

namespace D2AutoFisher
{
    public partial class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint FindWindowA(string? lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendMessage(nint hWnd, int wMsg, nint wParam, nint lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern nint GetMessageExtraInfo();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetLastError();
    }
}
