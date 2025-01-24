using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Utils
{
	[SecuritySafeCritical]
	internal static class Win32Api
	{
		private static class UnsafeNativeMethods
		{
			[return: MarshalAs(UnmanagedType.Bool)]
			internal delegate bool EnumChildProc([In] IntPtr hWnd, [In] IntPtr lParam);

			[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			internal static extern IntPtr FindWindow([Optional][In] string lpClassName, [Optional][In] string lpWindowName);

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool EnumChildWindows([Optional][In] IntPtr hWndParent, [In] EnumChildProc lpEnumFunc, [In] IntPtr lParam);

			[DllImport("user32.dll")]
			internal static extern int GetDlgCtrlID([In] IntPtr hDlg);

			[DllImport("user32.dll", SetLastError = true)]
			internal static extern uint GetDlgItemText([In] IntPtr hDlg, [In] int nIDDlgItem, [Out] StringBuilder lpString, [In] int nMaxCount);

			[DllImport("user32.dll")]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool SetDlgItemText([In] IntPtr hDlg, [In] int nIDDlgItem, [In] string lpString);

			[DllImport("user32.dll", SetLastError = true)]
			[return: MarshalAs(UnmanagedType.Bool)]
			internal static extern bool PostMessage([In] IntPtr hWnd, [In] uint Msg, [In] IntPtr wParam, [In] IntPtr lParam);
		}

		public static IntPtr FindMessageBox(string caption)
		{
			return UnsafeNativeMethods.FindWindow("#32770", caption);
		}

		public static void SendCommandToDlgButton(IntPtr hWnd, int dlgButtonId)
		{
			if (hWnd == IntPtr.Zero)
			{
				return;
			}
			UnsafeNativeMethods.EnumChildWindows(hWnd, delegate(IntPtr handle, IntPtr param)
			{
				int dlgCtrlID = UnsafeNativeMethods.GetDlgCtrlID(handle);
				if (dlgCtrlID == dlgButtonId)
				{
					UnsafeNativeMethods.PostMessage(hWnd, 273u, new IntPtr(dlgCtrlID), handle);
				}
				return dlgCtrlID != dlgButtonId;
			}, IntPtr.Zero);
		}

		public static string GetDlgButtonText(IntPtr hWnd, int dlgButtonId)
		{
			if (hWnd == IntPtr.Zero)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder(128);
			int dlgItemText = (int)UnsafeNativeMethods.GetDlgItemText(hWnd, dlgButtonId, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString(0, dlgItemText);
		}

		public static bool SetDlgButtonText(IntPtr hWnd, int dlgButtonId, string text)
		{
			if (hWnd == IntPtr.Zero)
			{
				return false;
			}
			return UnsafeNativeMethods.SetDlgItemText(hWnd, dlgButtonId, text);
		}
	}
}
