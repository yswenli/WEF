/****************************************************************************
*Copyright (c) YSWenli All Rights Reserved.
*CLR版本： .net8.0
*机器名称：WALLE
*Author：yswenli
*命名空间：WEF.Standard.DevelopTools.Common.Win32
*文件名： ScreenHelper
*版本号： V1.0.0.0
*唯一标识：d13a94e5-e0fa-456b-a8a2-3d87af681023
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：yswenli@outlook.com
*创建时间：2025/10/16 16:49:06
*描述：
*
*=================================================
*修改标记
*修改时间：2025/10/16 16:49:06
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WEF.Standard.DevelopTools.Common.Win32
{
	public class ScreenHelper
	{
		#region API 声明

		/// <summary>
		/// 禁用 DPI 虚拟化
		/// </summary>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool SetProcessDPIAware();

		/// <summary>
		/// 禁用 DPI 虚拟化
		/// </summary>
		/// <param name="dpiContext"></param>
		/// <returns></returns>
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiContext);

		// DPI 类型枚举
		public enum MONITOR_DPI_TYPE
		{
			MDT_EFFECTIVE_DPI = 0, // 有效 DPI（系统缩放后的 DPI）
			MDT_ANGULAR_DPI = 1,   // 物理 DPI（硬件分辨率对应的 DPI）
			MDT_RAW_DPI = 2        // 原始 DPI（未缩放的硬件 DPI）
		}

		/// <summary>
		/// Windows API：获取指定屏幕的 DPI
		/// </summary>
		/// <param name="hmonitor"></param>
		/// <param name="dpiType"></param>
		/// <param name="dpiX"></param>
		/// <param name="dpiY"></param>
		/// <returns></returns>
		[DllImport("shcore.dll")]
		private static extern int GetDpiForMonitor(IntPtr hmonitor, MONITOR_DPI_TYPE dpiType, out uint dpiX, out uint dpiY);

		/// <summary>
		/// 1. 根据坐标获取包含该点的屏幕句柄（最常用）
		/// </summary>
		/// <param name="pt">系统全局坐标（如鼠标位置、屏幕左上角）</param>
		/// <param name="dwFlags">筛选规则：无匹配时的 fallback 策略</param>
		/// <returns>屏幕句柄（HMONITOR），失败返回 IntPtr.Zero</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr MonitorFromPoint(Point pt, uint dwFlags);

		/// <summary>
		/// 2. 根据窗口句柄获取窗口所在的屏幕句柄
		/// </summary>
		/// <param name="hwnd">目标窗口句柄（如 WinForm 的 Form.Handle）</param>
		/// <param name="dwFlags">筛选规则：按窗口与屏幕的重叠比例判断</param>
		/// <returns>屏幕句柄（HMONITOR），失败返回 IntPtr.Zero</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

		/// <summary>
		/// 3. 枚举系统中所有屏幕（需配合回调函数）
		/// </summary>
		/// <param name="hdc">绘图上下文（为 IntPtr.Zero 时枚举所有屏幕）</param>
		/// <param name="lprcClip">裁剪区域（为 IntPtr.Zero 时不限制区域）</param>
		/// <param name="lpfnEnum">回调函数：每枚举一个屏幕就执行一次</param>
		/// <param name="dwData">传递给回调函数的自定义数据（可选）</param>
		/// <returns>枚举成功返回 true，失败返回 false</returns>
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern bool EnumDisplayMonitors(
			IntPtr hdc,
			IntPtr lprcClip,
			MonitorEnumProc lpfnEnum,
			IntPtr dwData
		);

		/// <summary>
		/// 枚举屏幕的回调函数委托（与 EnumDisplayMonitors 配合使用）
		/// </summary>
		/// <param name="hMonitor">当前枚举到的屏幕句柄</param>
		/// <param name="hdcMonitor">屏幕的绘图上下文（通常无需使用）</param>
		/// <param name="lprcMonitor">屏幕的边界矩形（全局坐标）</param>
		/// <param name="dwData">自定义数据（来自 EnumDisplayMonitors 的 dwData 参数）</param>
		/// <returns>返回 true 继续枚举，返回 false 停止枚举</returns>
		public delegate bool MonitorEnumProc(
			IntPtr hMonitor,
			IntPtr hdcMonitor,
			ref Rectangle lprcMonitor,
			IntPtr dwData
		);

		#endregion

		#region 筛选规则常量（dwFlags 参数取值）
		/// 无匹配时返回 IntPtr.Zero（无效句柄）
		public const uint MONITOR_DEFAULTTONULL = 0x00000000;
		/// 无匹配时返回主屏幕句柄
		public const uint MONITOR_DEFAULTTOPRIMARY = 0x00000001;
		/// 无匹配时返回最近的屏幕句柄
		public const uint MONITOR_DEFAULTTONEAREST = 0x00000002;
		/// 窗口与屏幕无重叠时返回 IntPtr.Zero（仅 MonitorFromWindow 使用）
		public const uint MONITOR_WINDOW_NOFIT = 0x00000004;
		#endregion

		/// <summary>
		/// 使用完毕后释放DC资源
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="hDC"></param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);


		/// <summary>
		/// 根据坐标获取屏幕句柄
		/// </summary>
		/// <param name="targetPoint">系统全局坐标（如 new Point(0,0) 为主屏幕左上角）</param>
		/// <returns>屏幕句柄，失败返回 IntPtr.Zero</returns>
		public static IntPtr GetMonitorHandleByPoint(Point targetPoint)
		{
			// 调用 API：根据坐标获取屏幕句柄，无匹配时返回主屏幕
			IntPtr hMonitor = MonitorFromPoint(
				pt: targetPoint,
				dwFlags: MONITOR_DEFAULTTOPRIMARY // 无匹配时 fallback 到主屏幕
			);
			return hMonitor;
		}

		/// <summary>
		/// 根据屏幕位置获取屏幕的DC
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public static IntPtr GetDC(Screen screen)
		{
			return GetMonitorHandleByPoint(screen.Bounds.Location);
		}

		/// <summary>
		/// 根据屏幕获取屏幕的DPI比例
		/// </summary>
		/// <param name="screen"></param>
		/// <returns></returns>
		public static float GetScreenDpiScale(Screen screen)
		{
			IntPtr hMonitor = GetDC(screen);
			GetDpiForMonitor(hMonitor, MONITOR_DPI_TYPE.MDT_RAW_DPI, out uint dpiX, out uint dpiY);
			ReleaseDC(IntPtr.Zero, hMonitor);
			var result = (float)dpiX / 96f;
			//禁用dpi虚拟化ui就变形了，不禁用就无法获取真的dpi数量，改成传入的值
			if (result > 1f)
			{
				result = UtilsHelper.GetSysconfigModel().DpiScale;
			}
			return result;
		}

		/// <summary>
		/// 根据屏幕句柄获取屏幕的DPI比例
		/// </summary>
		/// <param name="rectangle"></param>
		/// <returns></returns>
		public static List<float> GetDpiScaleList()
		{
			var result = new List<float>();
			var screens = Screen.AllScreens;
			if (screens == null || screens.Length < 1)
			{
				screens = new Screen[] { Screen.PrimaryScreen };
			}
			foreach (Screen screen in screens)
			{
				result.Add(GetScreenDpiScale(screen));
			}
			return result;
		}




		/// <summary>
		/// 获取虚拟屏幕边界（包含所有显示器）
		/// </summary>
		/// <returns>虚拟屏幕边界</returns>
		public static Rectangle GetVirtualScreenBounds()
		{
			int minX = int.MaxValue;
			int minY = int.MaxValue;
			int maxX = int.MinValue;
			int maxY = int.MinValue;

			// 确保获取所有可用的屏幕
			Screen[] allScreens = Screen.AllScreens;
			if (allScreens.Length == 0)
			{
				// 如果没有检测到屏幕，使用主屏幕
				allScreens = new Screen[] { Screen.PrimaryScreen };
			}

			foreach (Screen screen in allScreens)
			{
				Rectangle bounds = screen.Bounds;

				minX = Math.Min(minX, bounds.X);
				minY = Math.Min(minY, bounds.Y);

				var scale = GetScreenDpiScale(screen);
				var boundRight = (int)(bounds.Right * scale);
				var boundsBottom = (int)(bounds.Bottom * scale);

				maxX = Math.Max(maxX, boundRight);
				maxY = Math.Max(maxY, boundsBottom);
			}

			// 确保返回有效的矩形
			if (minX == int.MaxValue || minY == int.MaxValue || maxX == int.MinValue || maxY == int.MinValue)
			{
				// 如果计算失败，回退到主屏幕
				var scale = GetScreenDpiScale(Screen.PrimaryScreen);
				Rectangle primaryBounds = Screen.PrimaryScreen.Bounds;
				var primaryBoundsWidth = (int)(primaryBounds.Width * scale);
				var primaryBoundsHeight = (int)(primaryBounds.Height * scale);
				return new Rectangle(0, 0, primaryBoundsWidth, primaryBoundsHeight);
			}
			return new Rectangle(minX, minY, maxX - minX, maxY - minY + 1500);
		}


		/// <summary>
		/// 绘制屏幕
		/// </summary>
		/// <param name="bmp"></param>
		/// <param name="virtualScreen"></param>
		/// <param name="mousePosition"></param>
		/// <param name="captureCursor"></param>
		public static void DrawScreen(Bitmap bmp, Rectangle virtualScreen, Point mousePosition, bool captureCursor = true)
		{
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
				g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
				g.CopyFromScreen(virtualScreen.X, virtualScreen.Y, 0, 0, virtualScreen.Size);

				if (captureCursor)
				{
					MouseAndKeyHelper.CURSORINFO pci;
					pci.cbSize = Marshal.SizeOf(typeof(MouseAndKeyHelper.CURSORINFO));
					MouseAndKeyHelper.GetCursorInfo(out pci);
					if (pci.hCursor != IntPtr.Zero)
					{
						Cursor cur = new Cursor(pci.hCursor);
						Rectangle rect_cur = new Rectangle((Point)((Size)mousePosition - (Size)cur.HotSpot), cur.Size);
						cur.Draw(g, rect_cur);
					}
				}
			}
			//test
			bmp.Save("d:\\test.png", ImageFormat.Png);
		}





	}
}
