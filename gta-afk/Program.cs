using System;

namespace gta_afk
{
    class Program
    {
        static void Main(string[] args)
        {
            const string windowClassName = "Notepad";
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