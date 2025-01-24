namespace System.Windows.Forms.Extensions
{
	internal static class MessageBoxButtonsExtension
	{
		public static DialogResult ToDialogResult(this MessageBoxButtons buttons, DialogResult defaultResult)
		{
			switch (buttons)
			{
			case MessageBoxButtons.OK:
				return DialogResult.OK;
			case MessageBoxButtons.OKCancel:
				if (defaultResult != DialogResult.Cancel)
				{
					return DialogResult.OK;
				}
				break;
			case MessageBoxButtons.YesNo:
				if (defaultResult != DialogResult.No)
				{
					return DialogResult.Yes;
				}
				break;
			case MessageBoxButtons.YesNoCancel:
				if (defaultResult != DialogResult.No && defaultResult != DialogResult.Cancel)
				{
					return DialogResult.Yes;
				}
				break;
			case MessageBoxButtons.RetryCancel:
				if (defaultResult != DialogResult.Retry)
				{
					return DialogResult.Cancel;
				}
				break;
			case MessageBoxButtons.AbortRetryIgnore:
				if (defaultResult != DialogResult.Abort && defaultResult != DialogResult.Retry)
				{
					return DialogResult.Ignore;
				}
				break;
			}
			return defaultResult;
		}

		public static int ToDialogButtonId(this DialogResult result, MessageBoxButtons buttons)
		{
			if (buttons == MessageBoxButtons.OK)
			{
				return 2;
			}
			return (int)result;
		}
	}
}
