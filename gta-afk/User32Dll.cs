using System;
using System.Runtime.InteropServices;

namespace gta_afk
{
    public class User32Dll
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetForegroundWindow();
    }
}