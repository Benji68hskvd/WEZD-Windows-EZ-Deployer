namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	public class MixedCodeDocumentTextFragment : MixedCodeDocumentFragment
	{
		public string Text
		{
			get => base.FragmentText;
            set => base.FragmentText = value;
        }

		internal MixedCodeDocumentTextFragment(MixedCodeDocument doc)
			: base(doc, MixedCodeDocumentFragmentType.Text)
		{
		}
	}
}
