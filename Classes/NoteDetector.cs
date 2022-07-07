using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Emgu.CV;
using static Emgu.CV.CvInvoke;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace AuToES.Classes
{
	internal class NoteDetector
	{

		private static double superBottomSimi = 0.4;
		private static String superTemplatePath = @"D:\Projects\AuToES\AuToES\templates\super.png";
		private static Image<Bgr, byte> superTemplate = new Image<Bgr, byte>(superTemplatePath);
		private static Mat superGrayTemplate = new Mat();

		public struct NoteType
		{
			public string name;
			public int rRangeBottom;
			public int rRangeTop;
			public int bRangeBottom;
			public int bRangeTop;
			public int gRangeBottom;
			public int gRangeTop;
			public double coverage;

			public NoteType(string name, int rBottom, int rTop, int gBottom, int gTop, int bBottom, int bTop, double coverage)
			{
				this.name = name;
				rRangeBottom = rBottom;
				rRangeTop = rTop;
				gRangeBottom = gBottom;
				gRangeTop = gTop;
				bRangeBottom = bBottom;
				bRangeTop = bTop;
				this.coverage = coverage;
			}
		}

		public static NoteType[] noteTypes = {new NoteType("click", 0, 110, 145, 200, 250, 255, 0.1),
			new NoteType("lastEnd", 230, 245, 175, 195, 120, 150, 0.008),
			new NoteType("lastStart", 230, 255, 170, 210, 0, 80, 0.1),
			new NoteType("leftMove", 220, 250, 0, 90, 90, 150, 0.2),
			new NoteType("rightMove", 235, 255, 68, 140, 0, 90, 0.2),
			new NoteType("skill", 0, 100, 190, 235, 235, 255, 0.1),};

		private static Dictionary<String, NoteType> noteMap = new Dictionary<string, NoteType>{ { "click", noteTypes[0] },
			{ "lastEnd", noteTypes[1] },{ "lastStart", noteTypes[2] },{ "leftMove", noteTypes[3] },{ "rightMove", noteTypes[4] },{ "skill", noteTypes[5] }, };

		static NoteDetector()
		{
			CvtColor(superTemplate.Mat, superGrayTemplate, ColorConversion.Bgr2Gray);
		}

		public static long FindMatch(Image<Bgr, byte> image, Image<Bgr, byte> template)
		{
			Stopwatch watch;
			var res = new Mat();
			double minLoc = 0, maxLoc = 0;
			Point minPoint = new Point();
			Point maxPoint = new Point();

			watch = Stopwatch.StartNew();
			MatchTemplate(image, template, res, TemplateMatchingType.Ccoeff);
			watch.Stop();
			long matchTime = watch.ElapsedMilliseconds;
			MinMaxLoc(res, ref minLoc, ref maxLoc, ref minPoint, ref maxPoint);
			var imageToPreview = image.Clone();
			Rectangle(imageToPreview, new Rectangle(maxPoint, template.Size), new MCvScalar(0, 0, 255), 2);
			imageToPreview.Save(@"D:\Projects\AuToES\AuToES\outputs\preview.png");

			return matchTime;
		}

		public static double Compare(Image<Bgr, byte> image, Image<Bgr, byte> template, Mat temGray)
		{
			Stopwatch watch;
			watch = Stopwatch.StartNew();
			var imgGray = new Mat();

			CvtColor(image.Mat, imgGray, ColorConversion.Bgr2Gray);
			Image<Gray, byte> imgB = new Image<Gray, byte>(image.Size);
			Image<Gray, byte> imgG = new Image<Gray, byte>(image.Size);
			Image<Gray, byte> imgR = new Image<Gray, byte>(image.Size);
			EqualizeHist(image[0], imgB);
			EqualizeHist(image[1], imgR);
			EqualizeHist(image[2], imgG);

			Image<Gray, byte> temB = new Image<Gray, byte>(template.Size);
			Image<Gray, byte> temG = new Image<Gray, byte>(template.Size);
			Image<Gray, byte> temR = new Image<Gray, byte>(template.Size);
			EqualizeHist(template[0], temB);
			EqualizeHist(template[1], temR);
			EqualizeHist(template[2], temG);

			var histImg = new Mat();
			int[] channels = new int[] { 0 };
			float[] ranges = new float[] { 0, 255 };
			int[] histSize = new int[] { 256 };
			VectorOfMat vMatImgs = new VectorOfMat();
			vMatImgs.Push(imgGray);
			CvInvoke.CalcHist(vMatImgs, channels, new Mat(), histImg, histSize, ranges, false);
			var histTem = new Mat();
			VectorOfMat vMatTems = new VectorOfMat();
			vMatTems.Push(temGray);
			CvInvoke.CalcHist(vMatTems, channels, new Mat(), histTem, histSize, ranges, false);

			double dist = 0;
			dist = CvInvoke.CompareHist(histImg, histTem, HistogramCompMethod.Correl);
			return dist;
		}

		public static double imageContainNoteCoverage(String imagePath, String noteType)
		{
			Image<Bgr, byte> image = new Image<Bgr, byte>(imagePath);
			int width = image.Width, height = image.Height;
			NoteType noteTypeInfo = noteMap[noteType];

			int cnt = 0;
			var outputImage = new Image<Bgr, byte>(width, height);
			Stopwatch watch;
			watch = Stopwatch.StartNew();
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
				{
					Byte b = image.Data[i, j, 0];
					Byte g = image.Data[i, j, 1];
					Byte r = image.Data[i, j, 2];
					if (r <= noteTypeInfo.rRangeTop && r >= noteTypeInfo.rRangeBottom && g <= noteTypeInfo.gRangeTop &&
						g >= noteTypeInfo.gRangeBottom && b <= noteTypeInfo.bRangeTop && b >= noteTypeInfo.bRangeBottom)
					{
						cnt++;
						outputImage.Data[i, j, 0] = 255;
						outputImage.Data[i, j, 1] = 255;
						outputImage.Data[i, j, 2] = 255;
						/*						outputImage.Data[i, j, 2] = (byte)((noteTypeInfo.rRangeTop + noteTypeInfo.rRangeBottom)/2);
												outputImage.Data[i, j, 1] = (byte)((noteTypeInfo.gRangeTop + noteTypeInfo.gRangeBottom) / 2);
												outputImage.Data[i, j, 0] = (byte)((noteTypeInfo.bRangeTop + noteTypeInfo.bRangeBottom) / 2);*/
					}
					else
					{
						outputImage.Data[i, j, 0] = b;
						outputImage.Data[i, j, 1] = g;
						outputImage.Data[i, j, 2] = r;
					}
				}
			watch.Stop();
			/*Console.WriteLine(watch.ElapsedMilliseconds);*/

			outputImage.Save(@"D:\Projects\AuToES\AuToES\outputs\colorPreview.png");
			return (double)cnt / (width * height);
		}

		public static double imageContainNoteCoverage(Image<Bgr, byte> image, String noteType)
		{
			int width = image.Width, height = image.Height;
			NoteType noteTypeInfo = noteMap[noteType];

			int cnt = 0;
			var outputImage = new Image<Bgr, byte>(width, height);
			Stopwatch watch;
			watch = Stopwatch.StartNew();
			for (int i = 0; i < height; i++)
				for (int j = 0; j < width; j++)
				{
					Byte b = image.Data[i, j, 0];
					Byte g = image.Data[i, j, 1];
					Byte r = image.Data[i, j, 2];
					if (r <= noteTypeInfo.rRangeTop && r >= noteTypeInfo.rRangeBottom && g <= noteTypeInfo.gRangeTop &&
						g >= noteTypeInfo.gRangeBottom && b <= noteTypeInfo.bRangeTop && b >= noteTypeInfo.bRangeBottom)
					{
						cnt++;
						/*						outputImage.Data[i, j, 0] = 255;
												outputImage.Data[i, j, 1] = 255;
												outputImage.Data[i, j, 2] = 255;*/
						/*						outputImage.Data[i, j, 2] = (byte)((noteTypeInfo.rRangeTop + noteTypeInfo.rRangeBottom)/2);
												outputImage.Data[i, j, 1] = (byte)((noteTypeInfo.gRangeTop + noteTypeInfo.gRangeBottom) / 2);
												outputImage.Data[i, j, 0] = (byte)((noteTypeInfo.bRangeTop + noteTypeInfo.bRangeBottom) / 2);*/
					}
					else
					{
						outputImage.Data[i, j, 0] = b;
						outputImage.Data[i, j, 1] = g;
						outputImage.Data[i, j, 2] = r;
					}
				}
			watch.Stop();
			/*Console.WriteLine(watch.ElapsedMilliseconds);*/

			/*outputImage.Save(@"D:\Projects\AuToES\AuToES\outputs\colorPreview.png");*/
			return (double)cnt / (width * height);
		}

		public static String detectNote(String imagePath)
		{
			Image<Bgr, byte> image = new Image<Bgr, byte>(imagePath);
			return detectNote(image);
		}
		public static String detectNote(Image<Bgr, byte> image)
		{
			Stopwatch watch;
			watch = Stopwatch.StartNew();
			double superSimi = Compare(image, superTemplate, superGrayTemplate);
			if (superSimi > superBottomSimi)
			{
				watch.Stop();
				return "super";
			}
			foreach (KeyValuePair<string, NoteType> pair in noteMap)
			{
				double coverage = imageContainNoteCoverage(image, pair.Key);
/*				if(coverage > 0.15 || pair.Key=="lastEnd" && coverage>0)
				{
					Console.WriteLine("{0} detected, cover {1}", pair.Key, coverage);
				}*/
				if (coverage > pair.Value.coverage)
				{
					watch.Stop();
					return pair.Key;
				}
			}
			watch.Stop();
			return "";
		}
	}
}
