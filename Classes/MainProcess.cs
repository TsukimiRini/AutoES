using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Emgu.CV;
using Emgu.CV.Structure;
using static Emgu.CV.CvInvoke;
using Emgu.CV.CvEnum;

namespace AuToES.Classes
{
	internal class MainProcess
	{
		private static Point bias = new Point(156, 33);
		private static Point[] targets = {new Point(1216, 849), new Point (468, 711), new Point(696, 848), new Point(956,896),
		new Point(1444, 713), new Point(1609, 503), new Point(305, 502)};
		private static int cropSize = 160;

		private static bool clicked = false;
		private static int countDown = 0;
		private static int countDownMax = 3;
		private static int waitTime = 1000;
		private static bool targetDetected = false;

		private static Image<Bgr, byte> temp = new Image<Bgr, byte>(@"..\..\..\templates\target.png");

		private static IntPtr win;
		private static PSIZE emulatorSize;

		static MainProcess()
		{
			ToBinary(temp.Bitmap);
		}

		internal static void main(Form mainForm)
		{
			var process = Process.GetProcessesByName("dnplayer")[0];
			win = process.MainWindowHandle;
			IntPtr thumbnailPtr;

			WindowHandler.MappingWindow(mainForm, mainForm.Handle, win, out thumbnailPtr);
			DwmApi.DwmQueryThumbnailSourceSize(thumbnailPtr, out emulatorSize);

			ThreadStart targetCheck = new ThreadStart(checkTarget);
			Thread targetCheckThread = new Thread(targetCheck);
			targetCheckThread.Start();

			while (true)
			{
				while (targetDetected)
				{
					if (countDown == 1)
						clicked = false;
					countDown--;
					oneLoop(win, thumbnailPtr);
				}
				Thread.Sleep(waitTime);
			}

			/*AdbHandler.press(500, 500);*/
		}

		static int cnt = 0;
		internal static void oneLoop(IntPtr emulatorHandle, IntPtr thumbnailPtr)
		{
			PSIZE size;
			DwmApi.DwmQueryThumbnailSourceSize(thumbnailPtr, out size);

			Bitmap captured = WindowHandler.CaptureWindow(emulatorHandle, 0, 0, size.x, size.y);
			for (int i = 0; i < targets.Length; i++)
			{
				var target = targets[i];
				Bitmap targetCrop = Utils.cropBMP(captured, target.X - bias.X - cropSize / 2, target.Y - bias.Y - cropSize / 2, cropSize, cropSize);
				reactToNote(emulatorHandle, new Image<Bgr, byte>(targetCrop), new Point(target.X - bias.X, target.Y - bias.Y), i == 0);
			}
		}

		private static int targetCropSize = 100;
		private static int targetBottomHashSimi = 150;
		private static int targetBottomHashWhiteSimi = 25;
		internal static void checkTarget()
		{
			while (true)
			{
				Thread.Sleep(1000);
				// TODO: 优化，最好不要在子线程中重复截屏
				Bitmap captured = WindowHandler.CaptureWindow(win, 0, 0, emulatorSize.x, emulatorSize.y);
				int cnt = 0;
				for (int i = 0; i < targets.Length; i++)
				{
					var target = targets[i];
					Bitmap targetCrop = Utils.cropBMP(captured, target.X - bias.X - targetCropSize / 2, target.Y - bias.Y - targetCropSize / 2, targetCropSize, targetCropSize);
					ToBinary(targetCrop);

					int hashSimi, hashWhiteSimi;
					(hashSimi, hashWhiteSimi) = NoteDetector.HashGetEquals(targetCrop, temp.Bitmap);
					if (hashSimi > targetBottomHashSimi && hashWhiteSimi > targetBottomHashWhiteSimi)
					{
						cnt++;

						if (cnt >= 2)
						{
							if (targetDetected == false)
							{
								targetDetected = true;
								Console.WriteLine("target detected");
							}
							break;
						}
					}
					if (targetDetected && i == targets.Length - 1)
					{
						targetDetected = false;
						Console.WriteLine("target not detected");
					}
				}
			}
		}

		private static void ToBinary(Bitmap Bmp)
		{
			int rgb;
			Color c;

			for (int y = 0; y < Bmp.Height; y++)
				for (int x = 0; x < Bmp.Width; x++)
				{
					c = Bmp.GetPixel(x, y);
					if (c.R != 255 || c.G != 255 || c.B != 255) Bmp.SetPixel(x, y, Color.FromArgb(0, 0, 0));
				}
		}

		private static void reactToNote(IntPtr hwnd, Image<Bgr, Byte> cropImage, Point target, Boolean checkSuper)
		{
			switch (NoteDetector.detectNote(cropImage, checkSuper))
			{
				case "click":
				case "skill":
				case "lastStart":
					if (clicked && countDown != countDownMax) break;
					TouchEmulator.ClickOn(hwnd, target);
					/*AdbHandler.press(target.X, target.Y);*/
					clicked = true;
					countDown = countDownMax;
					break;
				case "lastEnd":
				case "super":
					TouchEmulator.ClickOn(hwnd, target);
					/*AdbHandler.press(target.X, target.Y);*/
					clicked = true;
					countDown = countDownMax;
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
