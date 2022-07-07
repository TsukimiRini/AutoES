using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuToES.Classes
{
	internal class WindowHandler
	{
		internal static void MappingWindow(Form mainForm, IntPtr tgt, IntPtr src, out IntPtr thumbnail)
		{
			if (DwmApi.DwmRegisterThumbnail(tgt, src, out thumbnail) == 0)
			{
				PSIZE size;
				DwmApi.DwmQueryThumbnailSourceSize(thumbnail, out size);

				RECT rect = new RECT();
				rect.Left = 0;
				rect.Right = size.x + rect.Left;
				rect.Top = 0;
				rect.Bottom = size.y + rect.Top;

				int margin = 20;
				mainForm.Size = new Size(size.x + margin, size.y + margin);

				DWM_THUMBNAIL_PROPERTIES properties = new DWM_THUMBNAIL_PROPERTIES();
				properties.dwFlags = (int)(DW_TNP.DWM_TNP_VISIBLE | DW_TNP.DWM_TNP_OPACITY | DW_TNP.DWM_TNP_RECTDESTINATION | DW_TNP.DWM_TNP_SOURCECLIENTAREAONLY);
				properties.fVisible = true;
				properties.opacity = 255;
				properties.rcDestination = rect;
				properties.fSourceClientAreaOnly = true;
				DwmApi.DwmUpdateThumbnailProperties(thumbnail, properties);
			}
			else
			{
				MessageBox.Show("未获取到缩略图，该程序不在任务栏或为后台应用");
			}
		}

		internal static Bitmap CaptureWindow(IntPtr hwnd, int x, int y, int width, int height)
		{
			var wdc = Win32.GetWindowDC(hwnd);
			var cdc = Win32.CreateCompatibleDC(wdc);
			var hBitmap = Win32.CreateCompatibleBitmap(wdc, width, height);
			var oldHBitmap = Win32.SelectObject(cdc, hBitmap);
			var result = Win32.BitBlt(cdc, x, y, width, height, wdc, x, y, (uint)RasterOperationMode.SRCCOPY);

			try
			{
				if (result)
				{
					Bitmap bmp = Image.FromHbitmap(hBitmap);
					return bmp;
				}
				else
				{
					throw new Exception();
				}
			}
			finally
			{
				Win32.SelectObject(cdc, oldHBitmap);
				Win32.DeleteObject(hBitmap);
				Win32.DeleteDC(cdc);
				Win32.ReleaseDC(hwnd, wdc);
			}
		}
	}
}
