namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	public class MixedCodeDocumentCodeFragment : MixedCodeDocumentFragment
	{
		private string _code;

		public string Code
		{
			get
			{
				if (_code == null)
				{
					_code = FragmentText.Substring(Doc.TokenCodeStart.Length, FragmentText.Length - Doc.TokenCodeEnd.Length - Doc.TokenCodeStart.Length - 1).Trim();
					if (_code.StartsWith("="))
					{
						_code = Doc.TokenResponseWrite + _code.Substring(1, _code.Length - 1);
					}
				}
				return _code;
			}
			set => _code = value;
        }

		internal MixedCodeDocumentCodeFragment(MixedCodeDocument doc)
			: base(doc, MixedCodeDocumentFragmentType.Code)
		{
		}
	}
}
