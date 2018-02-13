using System.Text.RegularExpressions;

namespace AutoWiki.Models
{
	internal class Link
	{
		private string _header;

		public string Text { get; set; }
		public string Page { get; set; }
		public string Markdown { get; set; }
		public string Header => _header ?? (_header = _ConvertToLink(Markdown));

		public override string ToString()
		{
			return $"[{Text}]({Page}#{Header})";
		}

		private static string _ConvertToLink(string content)
		{
			if (content == null) return null;

			var link = Regex.Replace(content, @"[^\w\s]", string.Empty);
			link = Regex.Replace(link, @"\s", "-");

			return link.ToLower();
		}
	}
}