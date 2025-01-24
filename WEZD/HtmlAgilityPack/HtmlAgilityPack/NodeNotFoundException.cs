namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	public class NodeNotFoundException : Exception
	{
		public NodeNotFoundException()
		{
		}

		public NodeNotFoundException(string message)
			: base(message)
		{
		}

		public NodeNotFoundException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
