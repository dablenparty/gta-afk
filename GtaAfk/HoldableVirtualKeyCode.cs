using WindowsInput.Native;

namespace GtaAfk
{
    public struct HoldableVirtualKeyCode
    {
        public VirtualKeyCode KeyCode { get; }
        public bool Hold { get; }

        public HoldableVirtualKeyCode(VirtualKeyCode keyCode, bool hold)
        {
            KeyCode = keyCode;
            Hold = hold;
        }

        public override string ToString()
        {
            return KeyCode.ToString();
        }
    }
}