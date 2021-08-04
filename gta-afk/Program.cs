using System;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace gta_afk
{
    class Program
    {
        /// <summary>
        /// Holds a key for a specified amount of time
        /// </summary>
        /// <param name="keyCode">Key to hold</param>
        /// <param name="simulator">The input simulator</param>
        /// <param name="timeoutInMillis">Time (in milliseconds) to hold the key for</param>
        private static void HoldKey(VirtualKeyCode keyCode, IInputSimulator simulator, int timeoutInMillis)
        {
            simulator.Keyboard.KeyDown(keyCode);
            simulator.Keyboard.Sleep(timeoutInMillis);
            simulator.Keyboard.KeyUp(keyCode);
        }
        
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
            const string windowName = "Grand Theft Auto V";
            var handle = User32Dll.FindWindow(null, windowName);
            if (handle != IntPtr.Zero && User32Dll.SetForegroundWindow(handle))
            {
                Console.WriteLine($"Set window with handle {handle} to foreground");
                HoldKey(movementKeys[0], inputSimulator, 3000); // hold key for 3 seconds
            }
            else
            {
                Console.WriteLine($"Window could not be set to foreground (handle: {handle})");
            }
        }
    }
}