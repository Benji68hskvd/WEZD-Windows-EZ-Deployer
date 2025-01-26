using System.Runtime.InteropServices;

namespace WEZD.HtmlAgilityPack.HtmlAgilityPack
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct IOLibrary
	{
		public static void CopyAlways(string source, string target)
		{
			if (File.Exists(source))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(target));
				MakeWritable(target);
				File.Copy(source, target, overwrite: true);
			}
		}

        private static void MakeWritable(string path)
		{
			if (File.Exists(path))
			{
				File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
			}
		}
	}
}
