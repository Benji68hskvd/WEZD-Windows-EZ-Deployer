using System.Collections;

namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	public class MixedCodeDocumentFragmentList : IEnumerable
	{
		public class MixedCodeDocumentFragmentEnumerator : IEnumerator
		{
			private int _index;

			private IList<MixedCodeDocumentFragment> _items;

			public MixedCodeDocumentFragment Current => _items[_index];

			object IEnumerator.Current => Current;

            public MixedCodeDocumentFragmentEnumerator(IList<MixedCodeDocumentFragment> items)
			{
				_items = items;
				_index = -1;
			}

			public bool MoveNext()
			{
				_index++;
				return _index < _items.Count;
			}

			public void Reset()
			{
				_index = -1;
			}
		}

		private MixedCodeDocument _doc;

		private IList<MixedCodeDocumentFragment> _items = new List<MixedCodeDocumentFragment>();

		public MixedCodeDocument Doc => _doc;

		public int Count => _items.Count;

		public MixedCodeDocumentFragment this[int index] => _items[index];

        public MixedCodeDocumentFragmentList(MixedCodeDocument doc)
		{
			_doc = doc;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public void Append(MixedCodeDocumentFragment newFragment)
		{
			if (newFragment == null)
			{
				throw new ArgumentNullException("newFragment");
			}
			_items.Add(newFragment);
		}

		public MixedCodeDocumentFragmentEnumerator GetEnumerator()
		{
			return new MixedCodeDocumentFragmentEnumerator(_items);
		}

		public void Prepend(MixedCodeDocumentFragment newFragment)
		{
			if (newFragment == null)
			{
				throw new ArgumentNullException("newFragment");
			}
			_items.Insert(0, newFragment);
		}

		public void Remove(MixedCodeDocumentFragment fragment)
		{
			if (fragment == null)
			{
				throw new ArgumentNullException("fragment");
			}
			int fragmentIndex = GetFragmentIndex(fragment);
			if (fragmentIndex == -1)
			{
				throw new IndexOutOfRangeException();
			}
			RemoveAt(fragmentIndex);
		}

		public void RemoveAll()
		{
			_items.Clear();
		}

		public void RemoveAt(int index)
		{
			_items.RemoveAt(index);
		}

        public void Clear()
		{
			_items.Clear();
		}

        private int GetFragmentIndex(MixedCodeDocumentFragment fragment)
		{
			if (fragment == null)
			{
				throw new ArgumentNullException("fragment");
			}
			for (int i = 0; i < _items.Count; i++)
			{
				if (_items[i] == fragment)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
