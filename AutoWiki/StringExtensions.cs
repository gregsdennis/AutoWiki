using System.IO;

namespace AutoWiki
{
	internal static class StringExtensions
	{
		public static string ChangeExtension(this string fileName, string newExtension)
		{
			var directory = Path.GetDirectoryName(fileName);
			var rawFileName = Path.GetFileNameWithoutExtension(fileName);

			return Path.Combine(directory, $"{rawFileName}.{newExtension}");
		}

		public static string AddTableOfContents(this string page)
		{
			return page;
		}
	}
}