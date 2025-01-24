namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	public class InvalidNodeReturnTypeException : Exception
	{
		public InvalidNodeReturnTypeException(string message)
			: base(message)
		{
		}

		public InvalidNodeReturnTypeException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
