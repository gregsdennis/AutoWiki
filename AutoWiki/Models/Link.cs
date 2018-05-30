using System.Linq;
using System.Text.RegularExpressions;

namespace AutoWiki.Models
{
	internal class Link
	{
		public static readonly Regex LinkPattern = new Regex(@"\[(?<name>.*?)]\(.*?\)");


		private string _header;

		public string Text { get; set; }
		public string Page { get; set; }
		public string Markdown { get; set; }
		public string Header => _header ?? (_header = _ConvertToLink(Markdown));

		public override string ToString()
		{
			var relative = Options.UseGitHubPagesLinks ? "../" : null;

			return $"[{Text}]({relative}{Page}#{Header})";
		}

		private static string _ConvertToLink(string content)
		{
			if (content == null) return null;

			string link = content;
			var matches = LinkPattern.Matches(content);
			var match = matches.FirstOrDefault();
			if (match?.Groups["name"].Success ?? false)
			{
				var name = match.Groups["name"].Value;
				link = LinkPattern.Replace(link, name);
			}
			link = Regex.Replace(link, @"[^\w\s]", string.Empty);
			link = Regex.Replace(link, @"\s", "-");

			return link.ToLower();
		}
	}
}