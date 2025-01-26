using System.Text;

namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	public class EncodingFoundException : Exception
	{
		private Encoding _encoding;

		public Encoding Encoding => _encoding;

		public EncodingFoundException(Encoding encoding)
		{
			_encoding = encoding;
		}
	}
}
