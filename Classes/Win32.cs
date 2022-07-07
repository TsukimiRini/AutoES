using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AuToES.Classes
{
	internal class Win32
	{
		[DllImport("user32.dll")]
		public static extern IntPtr GetWindowDC(IntPtr hWnd);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDc);

		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hDc, int nWidth, int nHeight);

		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDc, IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[DllImport("gdi32.dll")]
		public static extern bool DeleteDC(IntPtr hDc);

		[DllImport("user32.dll")]
		public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("gdi32.dll")]
		public static extern bool BitBlt(
			IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight,
			IntPtr hObjectSource, int nXSrc, int nYSrc, uint dwRop);

		[DllImport("user32.dll")]
		public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

		[DllImport("user32.dll")]
		public static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

		[DllImport("user32.dll")]
		public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("User32.dll")]
		public static extern Int32 SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetActiveWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();
	}

	internal enum RasterOperationMode : uint
	{
		SRCCOPY = 0x00CC0020,
		SRCPAINT = 0x00EE0086,
		SRCAND = 0x008800C6,
		SRCINVERT = 0x00660046,
		SRCERASE = 0x00440328,
		NOTSRCCOPY = 0x00330008,
		NOTSRCERASE = 0x001100A6,
		MERGECOPY = 0x00C000CA,
		MERGEPAINT = 0x00BB0226,
		PATCOPY = 0x00F00021,
		PATPAINT = 0x00FB0A09,
		PATINVERT = 0x005A0049,
		DSTINVERT = 0x00550009,
		BLACKNESS = 0x00000042,
		WHITENESS = 0x00FF0062,
		CAPTUREBLT = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
	}

	internal struct INPUT
	{
		public UInt32 Type;
		public MOUSEKEYBDHARDWAREINPUT Data;
	}

	[StructLayout(LayoutKind.Explicit)]
	internal struct MOUSEKEYBDHARDWAREINPUT
	{
		[FieldOffset(0)]
		public MOUSEINPUT Mouse;
	}

	internal struct MOUSEINPUT
	{
		public Int32 X;
		public Int32 Y;
		public UInt32 MouseData;
		public UInt32 Flags;
		public UInt32 Time;
		public IntPtr ExtraInfo;
	}

	internal enum EventCode
	{
		WM_MOUSEMOVE=0x0200,
		WM_LBUTTONDOWN = 0x0201,
		WM_LBUTTONUP= 0x0202
	}
}
