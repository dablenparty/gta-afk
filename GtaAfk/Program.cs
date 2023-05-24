using System;
using System.Collections.Generic;
using System.Linq;
using WindowsInput;
using WindowsInput.Native;

namespace GtaAfk;

public static class Program
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
    ///     Holds a key for a specified amount of time
    /// </summary>
    /// <param name="keyCode">Key to hold</param>
    /// <param name="timeoutInMillis">Time (in milliseconds) to hold the key for</param>
    private static void HoldKey(VirtualKeyCode keyCode, int timeoutInMillis)
    {
        HoldKey(new[] {keyCode}, timeoutInMillis);
    }

    /// <summary>
    ///     Holds multiple keys for a specified amount of time
    /// </summary>
    /// <param name="keyCodesEnumerable">Keys to hold</param>
    /// <param name="timeoutInMillis">Time (in milliseconds) to hold the keys for</param>
    private static void HoldKey(IEnumerable<VirtualKeyCode> keyCodesEnumerable, int timeoutInMillis)
    {
        var keyCodesArray = keyCodesEnumerable as VirtualKeyCode[] ?? keyCodesEnumerable.ToArray();
        var tail = timeoutInMillis == 1000 ? "second" : "seconds";
        Console.Write(
            $"\rHolding {string.Join(", ", keyCodesArray)} for {Convert.ToDecimal(timeoutInMillis) / 1000} {tail}"
                .PadRight(Console.WindowWidth));
        foreach (var keyCode in keyCodesArray) InputSimulatorInstance.Keyboard.KeyDown(keyCode);
        InputSimulatorInstance.Keyboard.Sleep(timeoutInMillis);
        foreach (var keyCode in keyCodesArray) InputSimulatorInstance.Keyboard.KeyUp(keyCode);
    }

    public static void Main(string[] args)
    {
        if (args.Length > 1) throw new ArgumentException("Too many args");
        var windowName = args.Length == 0 ? "Grand Theft Auto V" : args[0];
        var windowHandle = User32Dll.FindWindow(null, windowName);
        Console.WriteLine("Either close this window or click in and press Ctrl+C or Escape to exit");
        while (true)
        {
            var foregroundWindow = User32Dll.GetForegroundWindow();
            if (foregroundWindow != windowHandle)
                continue;
            // random delay between 1 and 4 seconds in half second intervals
            var delay = RandomInstance.Next(2, 9) * 500;
            var keyCount = RandomInstance.Next(1, 3);
            var keyCodes = GenerateMultipleRandomKeys(keyCount);
            var shouldSprint = RandomInstance.Next(0, 2) == 1;
            if (shouldSprint)
                keyCodes = keyCodes.Append(VirtualKeyCode.LSHIFT);

            HoldKey(keyCodes, delay);

            Console.Write("\rPress Esc to exit...".PadRight(Console.WindowWidth));
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                break;
        }
    }

    private static IEnumerable<VirtualKeyCode> GenerateMultipleRandomKeys(int count)
    {
        var keyCodes = new VirtualKeyCode[count];
        var i = 0;
        while (i < keyCodes.Length)
        {
            var key = GetRandomMovementKey();
            // don't use the same key twice
            if (keyCodes.Contains(key))
                continue;
            // disallows duplicate keys that would cancel each other out
            switch (key)
            {
                case VirtualKeyCode.VK_W:
                    if (keyCodes.Contains(VirtualKeyCode.VK_S))
                        continue;
                    break;
                case VirtualKeyCode.VK_S:
                    if (keyCodes.Contains(VirtualKeyCode.VK_W))
                        continue;
                    break;
                case VirtualKeyCode.VK_A:
                    if (keyCodes.Contains(VirtualKeyCode.VK_D))
                        continue;
                    break;
                case VirtualKeyCode.VK_D:
                    if (keyCodes.Contains(VirtualKeyCode.VK_A))
                        continue;
                    break;
                default:
                    Console.WriteLine($"Somehow generated an invalid key: {key}");
                    continue;
            }

            keyCodes[i++] = key;
        }

        return keyCodes;
    }

    private static VirtualKeyCode GetRandomMovementKey()
    {
        return MovementKeys[RandomInstance.Next(0, MovementKeys.Length)];
    }
}