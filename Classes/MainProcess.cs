using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;

namespace AuToES.Classes
{
	internal class MainProcess
	{
		private static Point bias = new Point(139, 76);
		private static Point[] targets = {new Point(286, 545), new Point (403, 707), new Point(560, 832), new Point(743,911),
		new Point(939, 937), new Point(1136, 911), new Point(1319, 834), new Point(1475, 709), new Point(1591, 548)};
		private static int cropSize = 160;

		internal static void main(Form mainForm)
		{
			var process = Process.GetProcessesByName("dnplayer")[0];
			IntPtr win = process.MainWindowHandle;
			IntPtr thumbnailPtr;

			WindowHandler.MappingWindow(mainForm, mainForm.Handle, win, out thumbnailPtr);
			while (true)
				oneLoop(win, thumbnailPtr);

		}

		internal static void oneLoop(IntPtr emulatorHandle, IntPtr thumbnailPtr)
		{
			PSIZE size;
			DwmApi.DwmQueryThumbnailSourceSize(thumbnailPtr, out size);

			Bitmap bmp = WindowHandler.CaptureWindow(emulatorHandle, 0, 0, size.x, size.y);
			foreach (Point target in targets)
			{
				Bitmap targetCrop = Utils.cropBMP(bmp, target.X - bias.X - cropSize / 2, target.Y - bias.Y - cropSize / 2, cropSize, cropSize);
				reactToNote(emulatorHandle, new Image<Bgr, byte>(targetCrop), new Point(target.X - bias.X, target.Y - bias.Y));
			}
		}

		private static void reactToNote(IntPtr hwnd, Image<Bgr, Byte> cropImage, Point target)
		{
			switch (NoteDetector.detectNote(cropImage))
			{
				case "click":
				case "lastStart":
				case "lastEnd":
				case "super":
				case "skill":
					TouchEmulator.ClickOn(hwnd, target);
					break;
				case "leftMove":
					TouchEmulator.MoveToLeft(hwnd, target);
					break;
				case "rightMove":
					TouchEmulator.MoveToRight(hwnd, target);
					break;
			}
		}
	}
}
