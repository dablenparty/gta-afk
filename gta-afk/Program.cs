using System;
using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;

namespace gta_afk
{
    class Program
    {
        private static readonly Random RandomInstance = new();
        private static readonly InputSimulator InputSimulatorInstance = new();
        private static readonly VirtualKeyCode[] MovementKeys =
        {
            VirtualKeyCode.VK_W, 
            VirtualKeyCode.VK_A, 
            VirtualKeyCode.VK_S, 
            VirtualKeyCode.VK_D
        };
        
        /// <summary>
        /// Holds a key for a specified amount of time
        /// </summary>
        /// <param name="keyCode">Key to hold</param>
        /// <param name="timeoutInMillis">Time (in milliseconds) to hold the key for</param>
        private static void HoldKey(VirtualKeyCode keyCode, int timeoutInMillis)
        {
            HoldKey(new []{keyCode}, timeoutInMillis);
        }

        /// <summary>
        /// Holds multiple keys for a specified amount of time
        /// </summary>
        /// <param name="keyCodesEnumerable">Keys to hold</param>
        /// <param name="timeoutInMillis">Time (in milliseconds) to hold the keys for</param>
        private static void HoldKey(IEnumerable<VirtualKeyCode> keyCodesEnumerable, int timeoutInMillis)
        {
            var keyCodesArray = keyCodesEnumerable as VirtualKeyCode[] ?? keyCodesEnumerable.ToArray();
            Console.WriteLine($"Holding {string.Join(", ", keyCodesArray)} for {timeoutInMillis} milliseconds");
            foreach (var keyCode in keyCodesArray) InputSimulatorInstance.Keyboard.KeyDown(keyCode);
            InputSimulatorInstance.Keyboard.Sleep(timeoutInMillis);
            foreach (var keyCode in keyCodesArray) InputSimulatorInstance.Keyboard.KeyUp(keyCode);
        }
        
        public static void Main(string[] args)
        {
            const string windowName = "Grand Theft Auto V";
            var handle = User32Dll.FindWindow(null, windowName);
            if (handle != IntPtr.Zero && User32Dll.SetForegroundWindow(handle))
            {
                while (User32Dll.GetForegroundWindow() == handle)
                {
                    var delay = RandomInstance.Next(2, 9) * 500; // random delay between 1 and 4 seconds in half second intervals
                    var holdMultiple = RandomInstance.Next(0, 2) == 1;
                    if (holdMultiple)
                    {
                        var keyCodes = new VirtualKeyCode[2];
                        for (var i = 0; i < keyCodes.Length; i++)
                        {
                            var key = GetRandomMovementKey();
                            while (keyCodes.Contains(key))
                            {
                                key = GetRandomMovementKey(); // don't hold the same key twice
                            }
                        
                            keyCodes[i] = key;
                        }
                        HoldKey(keyCodes, delay);
                    }
                    else
                    {
                        var key = GetRandomMovementKey();
                        HoldKey(key, delay);
                    }
                }
            }
            else
            {
                Console.WriteLine($"Window could not be set to foreground (handle: {handle})");
            }
        }

        private static VirtualKeyCode GetRandomMovementKey()
        {
            return MovementKeys[RandomInstance.Next(0, MovementKeys.Length)];
        }
    }
}