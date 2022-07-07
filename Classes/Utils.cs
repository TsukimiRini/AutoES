using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuToES.Classes
{
	internal class Utils
	{
		public static void saveBMP(Bitmap bmp, String path)
		{
			using (var ms = new MemoryStream())
			{
				bmp.Save(ms, ImageFormat.Png);
				ms.Seek(0, SeekOrigin.Begin);
				var data = ms.ToArray();
				bmp.Save(path, ImageFormat.Png);
			}
		}

		public static Bitmap cropBMP(Bitmap bmp, int x, int y, int width, int height)
		{
			Bitmap result = new Bitmap(width, height);
			using (Graphics g = Graphics.FromImage(result))
			{
				g.DrawImage(bmp, new Rectangle(0, 0, result.Width, result.Height),
					new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
			}
			return result;
		}
	}
}
