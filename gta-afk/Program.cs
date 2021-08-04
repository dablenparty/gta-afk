using System;
using WindowsInput;
using WindowsInput.Native;

namespace gta_afk
{
    class Program
    {
        public static void Main(string[] args)
        {
            var inputSimulator = new InputSimulator();
            VirtualKeyCode[] movementKeys =
            {
                VirtualKeyCode.VK_W, 
                VirtualKeyCode.VK_A, 
                VirtualKeyCode.VK_S, 
                VirtualKeyCode.VK_D,
                VirtualKeyCode.SPACE
            };
            const string windowClassName = "Grand Theft Auto V";
            var handle = User32Dll.FindWindow(windowClassName, null);
            if (handle != IntPtr.Zero && User32Dll.SetForegroundWindow(handle))
            {
                Console.WriteLine($"Set window with handle {handle} to foreground");
            }
            else
            {
                Console.WriteLine($"Window could not be set to foreground (handle: {handle})");
            }
        }
    }
}