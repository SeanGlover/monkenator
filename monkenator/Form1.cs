using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static monkenator.NativeMethods;

namespace monkenator
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private const int xHeartbeat = 1000 * 1;
        private static readonly System.Threading.Timer timer = new System.Threading.Timer(new TimerCallback(TokenTimer_tick), null, -1, -1);
        private void Form1_Load(object sender, EventArgs e)
        {
            //var surrClass = new SurroundingClass();
            //surrClass.MoveMouse(new Point(50, 50));
            //surrClass.ClickLeftMouseButton(new Point(50, 50));

            //MouseDown += Moused;
            //SurroundingClass.MouseRightclick(new Point(50, 50));
            //NativeMethods.SendInput()
            WindowState = FormWindowState.Maximized;
            ShowInTaskbar = false;
            SignIn_startHeartbeat();
        }
        public void SignIn_startHeartbeat() => timer.Change(0, xHeartbeat);
        private static void TokenTimer_tick(object sender) {
            Cursor.Position = new Point(50, 50);
            Thread.Sleep(1);
            Cursor.Position = new Point(49, 50);
        }
        private void Moused(object sender, EventArgs e) => Debugger.Break();
    }
    public static class SurroundingClass
    {
        [Flags]
        internal enum MouseEventFlags
        {
            MOUSEEVENTFxMOVE = 0x1,
            MOUSEEVENTFxLEFTDOWN = 0x2,
            MOUSEEVENTFxLEFTUP = 0x4,
            MOUSEEVENTFxRIGHTDOWN = 0x8,
            MOUSEEVENTFxRIGHTUP = 0x10,
            MOUSEEVENTFxMIDDLEDOWN = 0x20,
            MOUSEEVENTFxMIDDLEUP = 0x40,
            MOUSEEVENTFxXDOWN = 0x80,
            MOUSEEVENTFxXUP = 0x100,
            MOUSEEVENTFxWHEEL = 0x800,
            MOUSEEVENTFxVIRTUALDESK = 0x4000,
            MOUSEEVENTFxABSOLUTE = 0x8000
        }
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static void MouseRightclick(Point p)=> mouse_event((int)(MouseEventFlags.MOUSEEVENTFxRIGHTDOWN | MouseEventFlags.MOUSEEVENTFxRIGHTUP), p.X, p.Y, 0, 0);
        public static void MouseDoubleClick(Point p)
        {
            //mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.X, (uint)p.Y, 0, (UIntPtr)0);
            //Thread.Sleep(150);
            //mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.X, (uint)p.Y, 0, (UIntPtr)0);
        }

        public static void MouseRightDoubleClick(Point p)
        {
            //mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)p.X, (uint)p.Y, 0, (UIntPtr)0);
            //Thread.Sleep(150);
            //mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)p.X, (uint)p.Y, 0, (UIntPtr)0);
        }

        public static void MouseDown()=> mouse_event((int)MouseEventdwFlags.MOUSEEVENTF_LEFTDOWN , 50, 50, 0, 0);

        public static void MouseUp() => mouse_event((int)MouseEventdwFlags.MOUSEEVENTF_LEFTUP, 50, 50, 0, 0);

        internal enum SendInputEventType
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }
        [StructLayout(LayoutKind.Explicit)]
        internal struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            internal MouseInputData mi;
            [FieldOffset(0)]
            internal KEYBDINPUT ki;
            [FieldOffset(0)]
            internal HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            internal ushort wVk;
            internal ushort wScan;
            internal uint dwFlags;
            internal uint time;
            internal IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        internal struct MouseInputData
        {
            internal int dx;
            internal int dy;
            internal uint mouseData;
            internal MouseEventFlags dwFlags;
            internal uint time;
            internal IntPtr dwExtraInfo;
        }

        enum SystemMetric
        {
            SMxCXSCREEN = 0,
            SMxCYSCREEN = 1
        }
        private static int CalculateAbsoluteCoordinateX(int x) => Convert.ToInt32((x * 65536) / (double)GetSystemMetrics((int)SystemMetric.SMxCXSCREEN));
        private static int CalculateAbsoluteCoordinateY(int y) => Convert.ToInt32((y * 65536) / (double)GetSystemMetrics((int)SystemMetric.SMxCYSCREEN));
        public static void ClickLeftMouseButton(Point Location) => ClickLeftMouseButton(Location.X, Location.Y);
        public static void ClickLeftMouseButton(int x, int y)
        {
            INPUT MouseInput = new INPUT()
            {
                type = SendInputEventType.InputMouse
            };
            {
                var withBlock = MouseInput;
                withBlock.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
                withBlock.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
                withBlock.mkhi.mi.mouseData = 0;
                withBlock.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTFxMOVE | MouseEventFlags.MOUSEEVENTFxABSOLUTE;
                SendInput(1, MouseInput, Marshal.SizeOf(new INPUT()));
                withBlock.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTFxLEFTDOWN;
                SendInput(1, MouseInput, Marshal.SizeOf(new INPUT()));
                withBlock.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTFxLEFTUP;
                SendInput(1, MouseInput, Marshal.SizeOf(new INPUT()));
            }
        }
        public static void ClickRightMouseButton(Point Location) => ClickRightMouseButton(Location.X, Location.Y);
        public static void ClickRightMouseButton(int x, int y)
        {
            INPUT MouseInput = new INPUT()
            {
                type = SendInputEventType.InputMouse
            };
            {
                var withBlock = MouseInput;
                withBlock.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
                withBlock.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
                withBlock.mkhi.mi.mouseData = 0;
                withBlock.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTFxMOVE | MouseEventFlags.MOUSEEVENTFxABSOLUTE;
                SendInput(1, MouseInput, Marshal.SizeOf(new INPUT()));
                withBlock.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTFxRIGHTDOWN;
                SendInput(1, MouseInput, Marshal.SizeOf(new INPUT()));
                withBlock.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTFxRIGHTUP;
                SendInput(1, MouseInput, Marshal.SizeOf(new INPUT()));
            }
        }
        public static void MoveMouse(Point Location) => MoveMouse(Location.X, Location.Y);
        public static void MoveMouse(int x, int y)
        {
            INPUT MouseInput = new INPUT()
            {
                type = SendInputEventType.InputMouse
            };
            {
                var withBlock = MouseInput;
                withBlock.mkhi.mi.dx = CalculateAbsoluteCoordinateX(x);
                withBlock.mkhi.mi.dy = CalculateAbsoluteCoordinateY(y);
                withBlock.mkhi.mi.mouseData = 0;
                withBlock.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTFxMOVE | MouseEventFlags.MOUSEEVENTFxABSOLUTE;
                //NativeMethods.SendInput(1, new NativeMethods.INPUT(), Marshal.SizeOf(new INPUT()));
            }
        }
        public static void KeyPress(Keys keyCode)
        {
            INPUT input = new INPUT()
            {
                type = SendInputEventType.InputKeyboard,
                mkhi = new MouseKeybdhardwareInputUnion()
                {
                    ki = new KEYBDINPUT()
                    {
                        wVk = System.Convert.ToUInt16(keyCode),
                        wScan = 0,
                        dwFlags = 0,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };
            INPUT input2 = new INPUT()
            {
                type = SendInputEventType.InputKeyboard,
                mkhi = new MouseKeybdhardwareInputUnion()
                {
                    ki = new KEYBDINPUT()
                    {
                        wVk = System.Convert.ToUInt16(keyCode),
                        wScan = 0,
                        dwFlags = 2,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };
            var inputs = new INPUT[] { input, input2 };
            SendInput(Convert.ToUInt32(inputs.Length), inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        #region"dll imports"
        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        static extern uint SendInput(uint nInputs, INPUT pInputs, int cbSize);

        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        static extern uint SendInput(uint numberOfInputs, [In, MarshalAs(UnmanagedType.LPArray)] INPUT[] inputs, int sizeOfInputStructure);
        [DllImport("user32.dll")]

        internal static extern int GetSystemMetrics(int smIndex);
        #endregion
    }
    internal class NativeMethods
    {
        public static bool SendKeyboardInput(IntPtr hWnd, Keys key, Keys[] modifiers = null)
        {
            if (hWnd != IntPtr.Zero)
            {
                uint targetThreadID = GetWindowThreadProcessId(hWnd, IntPtr.Zero);
                uint currentThreadID = GetCurrentThreadId();

                if (targetThreadID != currentThreadID)
                {
                    try
                    {
                        if (!AttachThreadInput(currentThreadID, targetThreadID, true)) return false;
                        var parentWindow = GetAncestor(hWnd, GetAncestorFlags.GA_ROOT);
                        if (IsIconic(parentWindow))
                        {
                            if (!RestoreWindow(parentWindow)) return false;
                        }

                        if (!BringWindowToTop(parentWindow)) return false;
                        if (SetFocus(hWnd) == IntPtr.Zero) return false;
                    }
                    finally
                    {
                        AttachThreadInput(currentThreadID, targetThreadID, false);
                    }
                }
                else
                {
                    SetFocus(hWnd);
                }
            }

            var flagsKeyDw = IsExtendedKey(key) ? KeyboardInputFlags.ExtendedKey : KeyboardInputFlags.KeyDown;
            var flagsKeyUp = KeyboardInputFlags.KeyUp | (IsExtendedKey(key) ? KeyboardInputFlags.ExtendedKey : 0);

            var inputs = new List<INPUT>();
            var input = new INPUT(SendInputType.InputKeyboard);

            // Key Modifiers Down
            if (!(modifiers is null))
            {
                foreach (var modifier in modifiers)
                {
                    input.Union.Keyboard.Flags = KeyboardInputFlags.KeyDown;
                    input.Union.Keyboard.VirtKeys = (ushort)modifier;
                    inputs.Add(input);
                }
            }

            // Key Down
            input.Union.Keyboard.Flags = flagsKeyDw | KeyboardInputFlags.Unicode;
            input.Union.Keyboard.VirtKeys = (ushort)key;
            inputs.Add(input);

            // Key Up
            input.Union.Keyboard.Flags = flagsKeyUp | KeyboardInputFlags.Unicode;
            input.Union.Keyboard.VirtKeys = (ushort)key;
            inputs.Add(input);

            // Key Modifiers Up
            if (!(modifiers is null))
            {
                foreach (var modifier in modifiers)
                {
                    input.Union.Keyboard.Flags = KeyboardInputFlags.KeyUp;
                    input.Union.Keyboard.VirtKeys = (ushort)modifier;
                    inputs.Add(input);
                }
            }
            uint sent = SendInput((uint)inputs.Count(), inputs.ToArray(), Marshal.SizeOf<INPUT>());
            return sent > 0;
        }

        private readonly static Keys[] extendedKeys = { Keys.Up, Keys.Down, Keys.Left, Keys.Right, Keys.Home, Keys.End, Keys.Prior, Keys.Next, Keys.Insert, Keys.Delete };
        private static bool IsExtendedKey(Keys key) => extendedKeys.Contains(key);

        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-input
        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public SendInputType InputType;
            public InputUnion Union;

            public INPUT(SendInputType type)
            {
                InputType = type;
                Union = new InputUnion();
            }
        }

        public enum SendInputType : uint
        {
            InputMouse = 0,
            InputKeyboard = 1,
            InputHardware = 2
        }
        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;

            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;

            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventdwFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-keybdinput
        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort VirtKeys;
            public ushort wScan;
            public KeyboardInputFlags Flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        [Flags]
        public enum MouseEventdwFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }
        [Flags]
        public enum KeyboardInputFlags : uint
        {
            KeyDown = 0x0,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Scancode = 0x0008,
            Unicode = 0x0004
        }

        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/ns-winuser-windowplacement
        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWPLACEMENT
        {
            public int length;
            public WplFlags flags;
            public SW_Flags showCmd;
            public POINT ptMinPosition;
            public POINT ptMaxPosition;
            public RECT rcNormalPosition;
        }

        public enum WplFlags : uint
        {
            WPF_ASYNCWINDOWPLACEMENT = 0x0004,   // If the calling thread and the thread that owns the window are attached to different input queues, the system posts the request to the thread that owns the window. This prevents the calling thread from blocking its execution while other threads process the request.
            WPF_RESTORETOMAXIMIZED = 0x0002,     // The restored window will be maximized, regardless of whether it was maximized before it was minimized. This setting is only valid the next time the window is restored. It does not change the default restoration behavior.
                                                 // This flag is only valid when the SW_SHOWMINIMIZED value is specified for the showCmd member.
            WPF_SETMINPOSITION = 0x0001          // The coordinates of the minimized window may be specified. This flag must be specified if the coordinates are set in the ptMinPosition member.
        }
        [Flags]
        public enum SW_Flags : uint
        {
            SW_HIDE = 0X00,
            SW_SHOWNORMAL = 0x01,
            SW_MAXIMIZE = 0x03,
            SW_SHOWNOACTIVATE = 0x04,
            SW_SHOW = 0x05,
            SW_MINIMIZE = 0x06,
            SW_RESTORE = 0x09,
            SW_SHOWDEFAULT = 0x0A,
            SW_FORCEMINIMIZE = 0x0B
        }
        public enum GetAncestorFlags : uint
        {
            GA_PARENT = 1,     // Retrieves the parent window.This does not include the owner, as it does with the GetParent function.
            GA_ROOT = 2,       // Retrieves the root window by walking the chain of parent windows.
            GA_ROOTOWNER = 3   // Retrieves the owned root window by walking the chain of parent and owner windows returned by GetParent.
        }
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public Point ToPoint() => new Point(this.x, this.y);
            public PointF ToPointF() => new PointF((float)this.x, (float)this.y);
            public POINT FromPoint(Point p) => new POINT(p.X, p.Y);
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                Left = left; Top = top; Right = right; Bottom = bottom;
            }
            public Rectangle ToRectangle() => Rectangle.FromLTRB(Left, Top, Right, Bottom);
            public Rectangle ToRectangleOffset(POINT p) => Rectangle.FromLTRB(p.x, p.y, Right + p.x, Bottom + p.y);
            public RECT FromRectangle(RectangleF rectangle) => FromRectangle(Rectangle.Round(rectangle));
            public RECT FromRectangle(Rectangle rectangle) => new RECT()
            {
                Left = rectangle.Left,
                Top = rectangle.Top,
                Bottom = rectangle.Bottom,
                Right = rectangle.Right
            };
            public RECT FromXYWH(int x, int y, int width, int height) => new RECT(x, y, x + width, y + height);
        }

        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-getwindowplacement
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool GetWindowPlacement(IntPtr hWnd, [In, Out] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetWindowPlacement(IntPtr hWnd, [In] ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr voidProcessId);

        // https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-getcurrentthreadid
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern uint GetCurrentThreadId();

        // https://learn.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-attachthreadinput
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool AttachThreadInput([In] uint idAttach, [In] uint idAttachTo, [In, MarshalAs(UnmanagedType.Bool)] bool fAttach);

        [ResourceExposure(ResourceScope.None)]
        [DllImport("User32", ExactSpelling = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr GetAncestor(IntPtr hWnd, GetAncestorFlags flags);

        [DllImport("user32.dll")]
        internal static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr SetFocus(IntPtr hWnd);

        //https://learn.microsoft.com/en-us/windows/desktop/api/winuser/nf-winuser-sendinput
        [DllImport("user32.dll", SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint SendInput(uint nInputs, [In, MarshalAs(UnmanagedType.LPArray)] INPUT[] pInputs, int cbSize);

        public static bool RestoreWindow(IntPtr hWnd)
        {
            var wpl = new WINDOWPLACEMENT()
            {
                length = Marshal.SizeOf<WINDOWPLACEMENT>()
            };
            if (!GetWindowPlacement(hWnd, ref wpl)) return false;

            wpl.flags = WplFlags.WPF_ASYNCWINDOWPLACEMENT;
            wpl.showCmd = SW_Flags.SW_RESTORE;
            return SetWindowPlacement(hWnd, ref wpl);
        }
    }
}
