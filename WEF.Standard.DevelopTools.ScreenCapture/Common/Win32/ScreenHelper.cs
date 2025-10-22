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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WEF.Standard.DevelopTools.Common.Win32
{
    /// <summary>
    /// 屏幕与坐标辅助工具：封装多屏/DPI处理、虚拟屏幕边界与屏幕捕获。
    /// </summary>
    public class ScreenHelper
    {
        #region 性能优化 - 缓存静态数据

        private static Rectangle _cachedVirtualScreenBounds = Rectangle.Empty;
        private static DateTime _lastCacheTime = DateTime.MinValue;
        private static readonly TimeSpan _cacheValidDuration = TimeSpan.FromDays(365);

        #endregion

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
        /// <summary>
        /// 根据屏幕上的点获取其所在显示器的句柄（HMONITOR）
        /// </summary>
        /// <param name="targetPoint">屏幕坐标点</param>
        /// <returns>显示器句柄</returns>
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
        /// <summary>
        /// 获取指定屏幕的设备上下文（DC），用于原生绘制或DPI查询
        /// </summary>
        /// <param name="screen">目标屏幕</param>
        /// <returns>设备上下文句柄</returns>
        public static IntPtr GetDC(Screen screen)
        {
            return GetMonitorHandleByPoint(screen.Bounds.Location);
        }

        /// <summary>
        /// 根据屏幕获取屏幕的DPI比例
        /// </summary>
        /// <param name="screen"></param>
        /// <returns></returns>
        /// <summary>
        /// 获取指定屏幕的DPI缩放比例（相对96dpi），如125%返回1.25
        /// </summary>
        /// <param name="screen">目标屏幕</param>
        /// <returns>DPI缩放比例</returns>
        public static float GetScreenDpiScale(Screen screen)
        {
            IntPtr hMonitor = GetDC(screen);
            GetDpiForMonitor(hMonitor, MONITOR_DPI_TYPE.MDT_RAW_DPI, out uint dpiX, out uint dpiY);
            ReleaseDC(IntPtr.Zero, hMonitor);
            return (float)dpiX / 96f;
        }

        /// <summary>
        /// 根据屏幕句柄获取屏幕的DPI比例
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        /// <summary>
        /// 获取所有屏幕的DPI缩放比例列表，按屏幕顺序返回
        /// </summary>
        /// <returns>各屏幕的DPI缩放比例集合</returns>
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
        /// 获取虚拟屏幕边界（包含所有显示器）- 优化版本，支持缓存
        /// </summary>
        /// <returns>虚拟屏幕边界</returns>
        /// <summary>
        /// 获取虚拟屏幕的边界矩形（所有屏幕的联合区域）
        /// </summary>
        /// <returns>虚拟屏幕的全局坐标矩形</returns>
        public static Rectangle GetVirtualScreenBounds()
        {
            // 检查缓存是否有效
            if (!_cachedVirtualScreenBounds.IsEmpty &&
                DateTime.Now - _lastCacheTime < _cacheValidDuration)
            {
                return _cachedVirtualScreenBounds;
            }

            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            // 确保获取所有可用的屏幕
            Screen[] allScreens = Screen.AllScreens;
            if (allScreens == null || allScreens.Length == 0)
            {
                // 如果没有检测到屏幕，使用主屏幕
                allScreens = new Screen[] { Screen.PrimaryScreen };
            }

            // 优化：批量处理屏幕边界计算
            foreach (Screen screen in allScreens)
            {
                if (screen?.Bounds == Rectangle.Empty) continue;

                Rectangle bounds = screen.Bounds;
                minX = Math.Min(minX, bounds.X);
                minY = Math.Min(minY, bounds.Y);

                // 优化：避免重复计算DPI，使用简化的计算
                var scale = GetScreenDpiScaleFast(screen);
                var boundRight = (int)(bounds.Right * scale);
                var boundsBottom = (int)(bounds.Bottom * scale);

                maxX = Math.Max(maxX, boundRight);
                maxY = Math.Max(maxY, boundsBottom);
            }

            Rectangle result;
            // 确保返回有效的矩形
            if (minX == int.MaxValue || minY == int.MaxValue || maxX == int.MinValue || maxY == int.MinValue)
            {
                // 如果计算失败，回退到主屏幕
                var scale = GetScreenDpiScaleFast(Screen.PrimaryScreen);
                Rectangle primaryBounds = Screen.PrimaryScreen.Bounds;
                var primaryBoundsWidth = (int)(primaryBounds.Width * scale);
                var primaryBoundsHeight = (int)(primaryBounds.Height * scale);
                result = new Rectangle(0, 0, primaryBoundsWidth, primaryBoundsHeight);
            }
            else
            {
                result = new Rectangle(minX, minY, maxX - minX, maxY - minY + 1500);
            }

            // 缓存结果
            _cachedVirtualScreenBounds = result;
            _lastCacheTime = DateTime.Now;

            return result;
        }

        /// <summary>
        /// 快速获取屏幕DPI比例
        /// </summary>
        private static float GetScreenDpiScaleFast(Screen screen)
        {
            try
            {
                IntPtr hMonitor = GetDC(screen);
                if (hMonitor != IntPtr.Zero)
                {
                    GetDpiForMonitor(hMonitor, MONITOR_DPI_TYPE.MDT_RAW_DPI, out uint dpiX, out uint dpiY);
                    ReleaseDC(IntPtr.Zero, hMonitor);
                    return (float)dpiX / 96f;
                }
            }
            catch
            {
                // 发生异常时返回默认值
            }

            return 1.0f; // 默认DPI比例
        }


        /// <summary>
        /// 绘制屏幕
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="virtualScreen"></param>
        /// <param name="mousePosition"></param>
        /// <param name="captureCursor"></param>
        /// <summary>
        /// 将虚拟屏幕区域绘制到位图，并可选叠加鼠标指针
        /// </summary>
        /// <param name="bmp">目标位图（需与区域大小一致）</param>
        /// <param name="virtualScreen">虚拟屏幕区域</param>
        /// <param name="mousePosition">当前鼠标全局坐标</param>
        /// <param name="captureCursor">是否叠加鼠标指针</param>
        public static void CaptureScreen(Bitmap bmp, Rectangle virtualScreen, Point mousePosition, bool captureCursor = true)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                g.CopyFromScreen(virtualScreen.X, virtualScreen.Y, 0, 0, virtualScreen.Size);
                if (captureCursor)
                {
                    try
                    {
                        MouseAndKeyHelper.CURSORINFO pci;
                        pci.cbSize = Marshal.SizeOf(typeof(MouseAndKeyHelper.CURSORINFO));
                        MouseAndKeyHelper.GetCursorInfo(out pci);
                        if (pci.hCursor != IntPtr.Zero)
                        {
                            using (Cursor cur = new Cursor(pci.hCursor))
                            {
                                Rectangle rect_cur = new Rectangle((Point)((Size)mousePosition - (Size)cur.HotSpot), cur.Size);
                                cur.Draw(g, rect_cur);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"绘制鼠标光标失败: {ex.Message}");
                    }
                }
            }
        }

        // 坐标与矩形转换辅助（屏幕/客户端互转）
        /// <summary>
        /// 将屏幕坐标转换为指定控件的客户端坐标（支持多屏/DPI）
        /// </summary>
        /// <param name="ctrl">目标控件</param>
        /// <param name="screenPoint">屏幕坐标点</param>
        /// <returns>客户端坐标点</returns>
        public static Point ScreenToClient(Control ctrl, Point screenPoint)
        {
            return ctrl.PointToClient(screenPoint);
        }
        /// <summary>
        /// 将客户端坐标转换为屏幕坐标（支持多屏/DPI）
        /// </summary>
        /// <param name="ctrl">源控件</param>
        /// <param name="clientPoint">客户端坐标点</param>
        /// <returns>屏幕坐标点</returns>
        public static Point ClientToScreen(Control ctrl, Point clientPoint)
        {
            return ctrl.PointToScreen(clientPoint);
        }
        /// <summary>
        /// 将控件客户端矩形转换为屏幕矩形（自动考虑偏移与DPI）
        /// </summary>
        /// <param name="ctrl">源控件</param>
        /// <param name="clientRect">客户端矩形</param>
        /// <returns>对应的屏幕坐标矩形</returns>
        public static Rectangle ClientRectToScreen(Control ctrl, Rectangle clientRect)
        {
            return ctrl.RectangleToScreen(clientRect);
        }
        /// <summary>
        /// 将屏幕矩形转换为控件客户端矩形（支持多屏/DPI）
        /// </summary>
        /// <param name="ctrl">目标控件</param>
        /// <param name="screenRect">屏幕坐标矩形</param>
        /// <returns>对应的客户端坐标矩形</returns>
        public static Rectangle ScreenRectToClient(Control ctrl, Rectangle screenRect)
        {
            return ctrl.RectangleToClient(screenRect);
        }
        /// <summary>
        /// 获取当前鼠标在指定控件中的客户端坐标
        /// </summary>
        /// <param name="ctrl">目标控件</param>
        /// <returns>客户端坐标点</returns>
        public static Point GetCursorClient(Control ctrl)
        {
            return ctrl.PointToClient(Control.MousePosition);
        }
        /// <summary>
        /// 将给定的屏幕矩形裁剪至其所在的屏幕边界内（避免越界）
        /// </summary>
        /// <param name="ctrl">用于确定当前屏幕的控件</param>
        /// <param name="screenRect">屏幕坐标矩形</param>
        /// <returns>裁剪后的屏幕矩形</returns>
        public static Rectangle ClipToCurrentScreen(Control ctrl, Rectangle screenRect)
        {
            var bounds = Screen.FromControl(ctrl)?.Bounds ?? Screen.PrimaryScreen.Bounds;
            return Rectangle.Intersect(screenRect, bounds);
        }
    }
}
