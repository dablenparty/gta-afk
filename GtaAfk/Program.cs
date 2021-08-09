using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;

namespace GtaAfk
{
    public static class Program
    {
        private static readonly Random RandomInstance = new();
        private static readonly InputSimulator InputSimulatorInstance = new();
        private static readonly VirtualKeyCode[] MovementKeys =
        {
            VirtualKeyCode.VK_W, 
            VirtualKeyCode.VK_A, 
            VirtualKeyCode.VK_S, 
            VirtualKeyCode.VK_D,
            VirtualKeyCode.LSHIFT
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
            Console.WriteLine("Either close this window or click in and press Escape at any time to exit");
            while (true)
            {
                while (User32Dll.GetForegroundWindow() == handle)
                {
                    var delay = RandomInstance.Next(2, 9) * 500; // random delay between 1 and 4 seconds in half second intervals
                    var holdMultiple = RandomInstance.Next(0, 2) == 1;
                    if (holdMultiple)
                    {
                        var keyCodes = GenerateMultipleRandomKeys(2);
                        // disallows opposite keys to be pressed so that movement is always ensured
                        while ((keyCodes.Contains(VirtualKeyCode.VK_W) && keyCodes.Contains(VirtualKeyCode.VK_S)) 
                            || (keyCodes.Contains(VirtualKeyCode.VK_A) && keyCodes.Contains(VirtualKeyCode.VK_D)))
                        {
                            keyCodes = GenerateMultipleRandomKeys(2);
                        }
                        HoldKey(keyCodes, delay);
                    }
                    else
                    {
                        var key = GetRandomMovementKey();
                        HoldKey(key, delay);
                    }
                }

                Thread.Sleep(500); // wait a half-second before checking again
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) break;
            }
        }

        private static VirtualKeyCode[] GenerateMultipleRandomKeys(int count)
        {
            var keyCodes = new VirtualKeyCode[count];
            for (var i = 0; i < keyCodes.Length; i++)
            {
                var key = GetRandomMovementKey();
                // don't use the same key twice
                while (keyCodes.Contains(key)) key = GetRandomMovementKey();

                keyCodes[i] = key;
            }

            return keyCodes;
        }

        private static VirtualKeyCode GetRandomMovementKey()
        {
            return MovementKeys[RandomInstance.Next(0, MovementKeys.Length)];
        }
    }
}