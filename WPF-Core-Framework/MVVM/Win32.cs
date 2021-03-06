﻿using System;
using System.Runtime.InteropServices;

namespace MVVM
{
    /// <summary>
    ///
    /// </summary>
    public class Win32
    {
        /// <summary>
        /// Indicates the position of the cursor hot spot.
        /// </summary>
        public enum HitTest : int
        {
            /// <summary>
            /// On the screen background or on a dividing line between windows (same as HTNOWHERE,
            /// except that the DefWindowProc function produces a system beep to indicate an error).
            /// </summary>
            HTERROR = -2,

            /// <summary>
            /// In a window currently covered by another window in the same thread (the message will
            /// be sent to underlying windows in the same thread until one of them returns a code
            /// that is not HTTRANSPARENT).
            /// </summary>
            HTTRANSPARENT = -1,

            /// <summary>
            /// On the screen background or on a dividing line between windows.
            /// </summary>
            HTNOWHERE = 0,

            /// <summary>
            /// In a client area.
            /// </summary>
            HTCLIENT = 1,

            /// <summary>
            /// In a title bar.
            /// </summary>
            HTCAPTION = 2,

            /// <summary>
            /// In a window menu or in a Close button in a child window.
            /// </summary>
            HTSYSMENU = 3,

            /// <summary>
            /// In a size box (same as HTSIZE).
            /// </summary>
            HTGROWBOX = 4,

            /// <summary>
            /// In a size box (same as HTGROWBOX).
            /// </summary>
            HTSIZE = 4,

            /// <summary>
            /// In a menu.
            /// </summary>
            HTMENU = 5,

            /// <summary>
            /// In a horizontal scroll bar.
            /// </summary>
            HTHSCROLL = 6,

            /// <summary>
            /// In the vertical scroll bar.
            /// </summary>
            HTVSCROLL = 7,

            /// <summary>
            /// In a Minimize button.
            /// </summary>
            HTMINBUTTON = 8,

            /// <summary>
            /// In a Minimize button.
            /// </summary>
            HTREDUCE = 8,

            /// <summary>
            /// In a Maximize button.
            /// </summary>
            HTMAXBUTTON = 9,

            /// <summary>
            /// In a Maximize button.
            /// </summary>
            HTZOOM = 9,

            /// <summary>
            /// In the left border of a resizable window (the user can click the mouse to resize the
            /// window horizontally).
            /// </summary>
            HTLEFT = 10,

            /// <summary>
            /// In the right border of a resizable window (the user can click the mouse to resize
            /// the window horizontally).
            /// </summary>
            HTRIGHT = 11,

            /// <summary>
            /// In the upper-horizontal border of a window.
            /// </summary>
            HTTOP = 12,

            /// <summary>
            /// In the upper-left corner of a window border.
            /// </summary>
            HTTOPLEFT = 13,

            /// <summary>
            /// In the upper-right corner of a window border.
            /// </summary>
            HTTOPRIGHT = 14,

            /// <summary>
            /// In the lower-horizontal border of a resizable window (the user can click the mouse
            /// to resize the window vertically).
            /// </summary>
            HTBOTTOM = 15,

            /// <summary>
            /// In the lower-left corner of a border of a resizable window (the user can click the
            /// mouse to resize the window diagonally).
            /// </summary>
            HTBOTTOMLEFT = 16,

            /// <summary>
            /// In the lower-right corner of a border of a resizable window (the user can click the
            /// mouse to resize the window diagonally).
            /// </summary>
            HTBOTTOMRIGHT = 17,

            /// <summary>
            /// In the border of a window that does not have a sizing border.
            /// </summary>
            HTBORDER = 18,

            /// <summary>
            /// In a Close button.
            /// </summary>
            HTCLOSE = 20,

            /// <summary>
            /// In a Help button.
            /// </summary>
            HTHELP = 21,
        };

        // Posted when the user presses the left mouse button while the cursor is within the nonclient area of a window
        /// <summary>
        ///
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0x00A1;

        // Sends the specified message to a window or windows
        /// <summary>
        ///
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        // Sent to a window in order to determine what part of the window corresponds to a particular screen coordinate
        /// <summary>
        ///
        /// </summary>
        public const int WM_NCHITTEST = 0x0084;

        // Sent to a window when the size or position of the window is about to change
        /// <summary>
        ///
        /// </summary>
        public const int WM_GETMINMAXINFO = 0x0024;

        // Retrieves a handle to the display monitor that is nearest to the window
        /// <summary>
        ///
        /// </summary>
        public const int MONITOR_DEFAULTTONEAREST = 2;

        // Retrieves a handle to the display monitor
        /// <summary>
        ///
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        // RECT structure, Rectangle used by MONITORINFOEX

        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            ///
            /// </summary>
            public int Left;

            /// <summary>
            ///
            /// </summary>
            public int Top;

            /// <summary>
            ///
            /// </summary>
            public int Right;

            /// <summary>
            ///
            /// </summary>
            public int Bottom;
        }

        // MONITORINFOEX structure, Monitor information used by GetMonitorInfo function
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class MONITORINFOEX
        {
            /// <summary>
            ///
            /// </summary>
            public int cbSize;

            /// <summary>
            ///
            /// </summary>
            public RECT rcMonitor; // The display monitor rectangle

            /// <summary>
            ///
            /// </summary>
            public RECT rcWork; // The working area rectangle

            /// <summary>
            ///
            /// </summary>
            public int dwFlags;

            /// <summary>
            ///
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
            public char[] szDevice;
        }

        // Point structure, Point information used by MINMAXINFO structure
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            /// <summary>
            ///
            /// </summary>
            public int x;

            /// <summary>
            ///
            /// </summary>
            public int y;

            /// <summary>
            ///
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        // MINMAXINFO structure, Window's maximum size and position information
        /// <summary>
        ///
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            /// <summary>
            ///
            /// </summary>
            public POINT ptReserved;

            /// <summary>
            ///
            /// </summary>
            public POINT ptMaxSize; // The maximized size of the window

            /// <summary>
            ///
            /// </summary>
            public POINT ptMaxPosition; // The position of the maximized window

            /// <summary>
            ///
            /// </summary>
            public POINT ptMinTrackSize;

            /// <summary>
            ///
            /// </summary>
            public POINT ptMaxTrackSize;
        }

        // Get the working area of the specified monitor
        /// <summary>
        ///
        /// </summary>
        /// <param name="hmonitor"></param>
        /// <param name="monitorInfo"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX monitorInfo);
    }
}