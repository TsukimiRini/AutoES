using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AuToES.Classes
{
	internal class DwmApi
	{
		[DllImport("Dwmapi.dll")]
		public static extern int DwmRegisterThumbnail(IntPtr hwndDestination, IntPtr hwndSource, out IntPtr phThumbnailId);

		[DllImport("Dwmapi.dll")]
		public static extern int DwmUpdateThumbnailProperties(IntPtr hThumbnailId, DWM_THUMBNAIL_PROPERTIES ptnProperties);

		[DllImport("Dwmapi.dll")]
		public static extern int DwmQueryThumbnailSourceSize(IntPtr hThumbnailId, out PSIZE size);

		[DllImport("Dwmapi.dll")]
		public static extern int DwmUnregisterThumbnail(IntPtr hThumbnailId);
	}
}
