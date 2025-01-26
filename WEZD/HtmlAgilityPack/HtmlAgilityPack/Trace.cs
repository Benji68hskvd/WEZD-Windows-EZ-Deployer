namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	public class Trace
	{
        private static Trace _current;

        private static Trace Current
		{
			get
			{
				if (_current == null)
				{
					_current = new Trace();
				}
				return _current;
			}
		}

		private void WriteLineIntern(string message, string category)
		{
		}

		public static void WriteLine(string message, string category)
		{
			Current.WriteLineIntern(message, category);
		}
	}
}
