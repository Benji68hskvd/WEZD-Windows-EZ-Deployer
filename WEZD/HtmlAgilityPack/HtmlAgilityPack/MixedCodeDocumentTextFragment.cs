namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	public class MixedCodeDocumentTextFragment : MixedCodeDocumentFragment
	{
		public string Text
		{
			get => FragmentText;
            set => FragmentText = value;
        }

        public MixedCodeDocumentTextFragment(MixedCodeDocument doc)
			: base(doc, MixedCodeDocumentFragmentType.Text)
		{
		}
	}
}
