using System.Text;

namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	internal class EncodingFoundException : Exception
	{
		private Encoding _encoding;

		internal Encoding Encoding => _encoding;

		internal EncodingFoundException(Encoding encoding)
		{
			_encoding = encoding;
		}
	}
}
