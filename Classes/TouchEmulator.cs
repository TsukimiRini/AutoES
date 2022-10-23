using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AuToES.Classes
{
	internal class TouchEmulator
	{
		public static void ClickOn(IntPtr hwnd, Point clientPoint)
		{
			var oldPos = Cursor.Position;
			Win32.ClientToScreen(hwnd, ref clientPoint);
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

			var oldFgWin = Win32.GetForegroundWindow();

			var inputMouseDown = MouseDownInput();
			var inputMouseUp = MouseUpInput();
			Win32.SetForegroundWindow(hwnd);
			SendOutInputs(inputMouseDown, inputMouseUp);

			Cursor.Position = oldPos;
			/*Win32.SetForegroundWindow(oldFgWin);*/
		}

		public static void MoveToLeft(IntPtr hwnd, Point clientPoint)
		{
			var oldPos = Cursor.Position;
			Win32.ClientToScreen(hwnd, ref clientPoint);
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

			var oldFgWin = Win32.GetForegroundWindow();

			var inputMouseDown = MouseDownInput();
			var inputMouseMoveLeft = MouseMoveLeftInput();
			var inputMouseUp = MouseUpInput();
			Win32.SetForegroundWindow(hwnd);
			SendOutInputs(inputMouseDown);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseUp);

			Cursor.Position = oldPos;
			/*Win32.SetForegroundWindow(oldFgWin);*/
		}

		public static void MoveToRight(IntPtr hwnd, Point clientPoint)
		{
			var oldPos = Cursor.Position;
			Win32.ClientToScreen(hwnd, ref clientPoint);
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

			var oldFgWin = Win32.GetForegroundWindow();

			var inputMouseDown = MouseDownInput();
			var inputMouseMoveLeft = MouseMoveRightInput();
			var inputMouseUp = MouseUpInput();
			Win32.SetForegroundWindow(hwnd);
			SendOutInputs(inputMouseDown);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseUp);

			Cursor.Position = oldPos;
			/*Win32.SetForegroundWindow(oldFgWin);*/
		}

		public static void MoveToUp(IntPtr hwnd, Point clientPoint)
		{
			var oldPos = Cursor.Position;
			Win32.ClientToScreen(hwnd, ref clientPoint);
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

			var oldFgWin = Win32.GetForegroundWindow();

			var inputMouseDown = MouseDownInput();
			var inputMouseMoveLeft = MouseMoveUpInput();
			var inputMouseUp = MouseUpInput();
			Win32.SetForegroundWindow(hwnd);
			SendOutInputs(inputMouseDown);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseUp);

			Cursor.Position = oldPos;
		}

		public static void MoveToDown(IntPtr hwnd, Point clientPoint)
		{
			var oldPos = Cursor.Position;
			Win32.ClientToScreen(hwnd, ref clientPoint);
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

			var oldFgWin = Win32.GetForegroundWindow();

			var inputMouseDown = MouseDownInput();
			var inputMouseMoveLeft = MouseMoveDownInput();
			var inputMouseUp = MouseUpInput();
			Win32.SetForegroundWindow(hwnd);
			SendOutInputs(inputMouseDown);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseMoveLeft);
			Thread.Sleep(10);
			SendOutInputs(inputMouseUp);

			Cursor.Position = oldPos;
			/*Win32.SetForegroundWindow(oldFgWin);*/
		}

		public static void MouseDown(IntPtr hwnd, Point clientPoint)
		{
			var oldPos = Cursor.Position;
			Win32.ClientToScreen(hwnd, ref clientPoint);
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

			var oldFgWin = Win32.GetForegroundWindow();

			var inputMouseDown = MouseDownInput();
			Win32.SetForegroundWindow(hwnd);
			SendOutInputs(inputMouseDown);

			Cursor.Position = oldPos;
			/*Win32.SetForegroundWindow(oldFgWin);*/
		}

		public static void MouseUp(IntPtr hwnd, Point clientPoint)
		{
			var oldPos = Cursor.Position;
			Win32.ClientToScreen(hwnd, ref clientPoint);
			Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

			var oldFgWin = Win32.GetForegroundWindow();

			var inputMouseUp = MouseUpInput();
			Win32.SetForegroundWindow(hwnd);
			SendOutInputs(inputMouseUp);

			Cursor.Position = oldPos;
		}

		private static void SendOutInputs(params INPUT[] inputs)
		{
			Win32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
		}

		private static INPUT MouseDownInput()
		{
			var inputMouseDown = new INPUT();
			inputMouseDown.Type = 0;
			inputMouseDown.Data.Mouse.Flags = 0x0002;
			return inputMouseDown;
		}

		private static INPUT MouseUpInput()
		{
			var inputMouseDown = new INPUT();
			inputMouseDown.Type = 0;
			inputMouseDown.Data.Mouse.Flags = 0x0004;
			return inputMouseDown;
		}

		private static INPUT MouseMoveLeftInput(int x = 10)
		{
			var inputMouseDown = new INPUT();
			inputMouseDown.Type = 0;
			inputMouseDown.Data.Mouse.Flags = 0x0001;
			inputMouseDown.Data.Mouse.X = -x;
			return inputMouseDown;
		}

		private static INPUT MouseMoveRightInput(int x = 10)
		{
			var inputMouseDown = new INPUT();
			inputMouseDown.Type = 0;
			inputMouseDown.Data.Mouse.Flags = 0x0001;
			inputMouseDown.Data.Mouse.X = x;
			return inputMouseDown;
		}

		private static INPUT MouseMoveUpInput(int x = 10)
		{
			var inputMouseDown = new INPUT();
			inputMouseDown.Type = 0;
			inputMouseDown.Data.Mouse.Flags = 0x0001;
			inputMouseDown.Data.Mouse.Y = -x;
			return inputMouseDown;
		}

		private static INPUT MouseMoveDownInput(int x = 10)
		{
			var inputMouseDown = new INPUT();
			inputMouseDown.Type = 0;
			inputMouseDown.Data.Mouse.Flags = 0x0001;
			inputMouseDown.Data.Mouse.Y = x;
			return inputMouseDown;
		}

		private static IntPtr CreateLParam(int LoWord, int HiWord)
		{
			return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
		}
	}
}
