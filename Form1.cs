using AuToES.Classes;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading;

namespace AuToES
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			MainProcess.main(this);
			/*			var process = Process.GetProcessesByName("dnplayer")[0];
						IntPtr win = process.MainWindowHandle;
						IntPtr thumbnailPtr;

						WindowHandler.MappingWindow(this, this.Handle, win, out thumbnailPtr);

						PSIZE size;
						DwmApi.DwmQueryThumbnailSourceSize(thumbnailPtr, out size);

						Stopwatch watch;
						watch = Stopwatch.StartNew();
						Bitmap bmp = WindowHandler.CaptureWindow(win, 0, 0, size.x, size.y);

						Bitmap res = Utils.cropBMP(bmp, 286 - 139 - 80, 545 - 76 - 80, 160, 160);
						Console.
			
			
			(watch.ElapsedMilliseconds);
						Utils.saveBMP(res, @"D:\Projects\AuToES\AuToES\outputs\cropPreview.png");*/

			/*var process = Process.GetProcessesByName("dnplayer")[0];
			IntPtr win = process.MainWindowHandle;

			IntPtr thumbnailPtr;

			MappingWindow(this.Handle, win, out thumbnailPtr);

			PSIZE size;
			DwmApi.DwmQueryThumbnailSourceSize(thumbnailPtr, out size);

			
			CaptureWindow(win, 0, 0, size.x, size.y);

			TouchEmulator.ClickOn(win, new Point(200, 200));

			TouchEmulator.MoveToDown(win, new Point(200, 200));

			TouchEmulator.MoveToUp(win, new Point(220, 220));

			TouchEmulator.MoveToRight(win, new Point(200, 220));

			TouchEmulator.MoveToLeft(win, new Point(220, 200));*/

			/*long matchTime = TemplateMatch.FindMatch(@"C:\Users\10275\Pictures\Screenshots\фад╩╫ьм╪(24).png", @"D:\Projects\AutoEsSprites\clickBig.png");*/
			/*double dist = NoteDetector.Compare(@"D:\Projects\AutoESTest\Snipaste_2022-06-29_10-28-02.png", @"D:\Projects\AutoEsSprites\leftMoveBig.png");
			this.Text = dist.ToString();*/
			/*double cnt = NoteDetector.imageContainNoteCoverage(@"D:\Projects\AutoEsSprites\skill.png", "click");
			this.Text = cnt.ToString();*/
			/*			string noteType = NoteDetector.detectNote(@"D:\Projects\AutoESTest\Snipaste_2022-07-06_22-22-37.png");
						this.Text = noteType.ToString();*/
		}
	}
}