using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace AuToES.Classes
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct RECT
	{
		public int Left; //左边开始绘制位置
		public int Top;  //上面开始绘制位置
		public int Right;  //右边截止绘制位置
		public int Bottom;  //下边截止绘制位置
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct PSIZE
	{
		public int x;
		public int y;
	}

	internal enum DW_TNP
	{
		DWM_TNP_RECTDESTINATION = 0x00000001,
		DWM_TNP_RECTSOURCE = 0x00000002,
		DWM_TNP_OPACITY= 0x00000004,
		DWM_TNP_VISIBLE = 0x00000008,
		DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010
	}

	[StructLayout(LayoutKind.Sequential)]
	internal class DWM_THUMBNAIL_PROPERTIES
	{
		public int dwFlags;
		public RECT rcDestination;
		public RECT rcSource;
		public int opacity;
		public bool fVisible;
		public bool fSourceClientAreaOnly;
	}
}
