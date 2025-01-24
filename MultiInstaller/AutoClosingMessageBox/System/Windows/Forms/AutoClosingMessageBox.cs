using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms.Extensions;
using Utils;

namespace System.Windows.Forms
{
	public class AutoClosingMessageBox
	{
		private sealed class DelayedCloseUpStrategy
		{
			private readonly string caption;

			private readonly int dlgButtonId;

			public DelayedCloseUpStrategy(string caption, int dlgButtonId)
			{
				this.caption = caption;
				this.dlgButtonId = dlgButtonId;
			}

			public void Proceed()
			{
				Win32Api.SendCommandToDlgButton(Win32Api.FindMessageBox(caption), dlgButtonId);
			}
		}

		private sealed class CountDownStrategy
		{
			private readonly string caption;

			private readonly int dlgButtonId;

			private readonly Stopwatch stopwatch = new Stopwatch();

			private IntPtr hWndMsgBox;

			private string dlgButtonInitialText;

			private TimeSpan timeout;

			private int lastRemainingSeconds = -1;

			public CountDownStrategy(string caption, int dlgButtonId, int timeout)
			{
				this.caption = caption;
				this.dlgButtonId = dlgButtonId;
				this.timeout = TimeSpan.FromMilliseconds(timeout);
				stopwatch.Start();
			}

			public void Proceed()
			{
				TimeSpan timeSpan = timeout - stopwatch.Elapsed;
				if (hWndMsgBox == IntPtr.Zero)
				{
					hWndMsgBox = Win32Api.FindMessageBox(caption);
					dlgButtonInitialText = Win32Api.GetDlgButtonText(hWndMsgBox, dlgButtonId);
				}
				if (timeSpan.TotalMilliseconds < 0.0)
				{
					stopwatch.Stop();
					Win32Api.SendCommandToDlgButton(hWndMsgBox, dlgButtonId);
					return;
				}
				int num = (int)Math.Round(timeSpan.TotalSeconds);
				if (lastRemainingSeconds != num)
				{
					IntPtr hWnd = hWndMsgBox;
					int num2 = dlgButtonId;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 2);
					defaultInterpolatedStringHandler.AppendFormatted(dlgButtonInitialText);
					defaultInterpolatedStringHandler.AppendLiteral("(");
					defaultInterpolatedStringHandler.AppendFormatted(num);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					Win32Api.SetDlgButtonText(hWnd, num2, defaultInterpolatedStringHandler.ToStringAndClear());
					lastRemainingSeconds = num;
				}
			}
		}

		public interface IAutoClosingMessageBox
		{
			DialogResult Show(int timeout = 1000, MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None);
		}

		private sealed class Impl : IAutoClosingMessageBox
		{
			private readonly Func<int, MessageBoxButtons, DialogResult, DialogResult> getResult;

			internal Impl(Func<string, MessageBoxButtons, DialogResult> showMethod, string caption, bool showCountDown)
			{
				getResult = (int timeout, MessageBoxButtons buttons, DialogResult defaultResult) => new AutoClosingMessageBox(caption, timeout, showMethod, buttons, defaultResult, showCountDown).result;
			}

			DialogResult IAutoClosingMessageBox.Show(int timeout, MessageBoxButtons buttons, DialogResult defaultResult)
			{
				return getResult(timeout, buttons, defaultResult);
			}
		}

		private readonly string caption;

		private readonly DialogResult result;

		private AutoClosingMessageBox(string caption, int timeout, Func<string, MessageBoxButtons, DialogResult> showMethod, MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None, bool showCountDown = false)
		{
			this.caption = caption ?? string.Empty;
			result = buttons.ToDialogResult(defaultResult);
			if (!showCountDown)
			{
				DelayedCloseUpStrategy state = new DelayedCloseUpStrategy(caption, result.ToDialogButtonId(buttons));
				using (new System.Threading.Timer(OnTimerElapsed, state, timeout, -1))
				{
					result = showMethod(this.caption, buttons);
					return;
				}
			}
			CountDownStrategy state2 = new CountDownStrategy(caption, result.ToDialogButtonId(buttons), timeout);
			using (new System.Threading.Timer(OnCountDownTimer, state2, 50, 250))
			{
				result = showMethod(this.caption, buttons);
			}
		}

		private static void OnTimerElapsed(object state)
		{
			((DelayedCloseUpStrategy)state).Proceed();
		}

		private static void OnCountDownTimer(object state)
		{
			((CountDownStrategy)state).Proceed();
		}

		public static DialogResult Show(string text, string caption = null, int timeout = 1000, MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None, bool showCountDown = false)
		{
			return new AutoClosingMessageBox(caption, timeout, (string capt, MessageBoxButtons btns) => MessageBox.Show(text, capt, btns), buttons, defaultResult, showCountDown).result;
		}

		public static DialogResult Show(IWin32Window owner, string text, string caption = null, int timeout = 1000, MessageBoxButtons buttons = MessageBoxButtons.OK, DialogResult defaultResult = DialogResult.None, bool showCountDown = false)
		{
			return new AutoClosingMessageBox(caption, timeout, (string capt, MessageBoxButtons btns) => MessageBox.Show(owner, text, capt, btns), buttons, defaultResult, showCountDown).result;
		}

		public static IAutoClosingMessageBox Factory(Func<string, MessageBoxButtons, DialogResult> showMethod, string caption = null, bool showCountDown = false)
		{
			if (showMethod == null)
			{
				throw new ArgumentNullException("showMethod");
			}
			return new Impl(showMethod, caption, showCountDown);
		}
	}
}
