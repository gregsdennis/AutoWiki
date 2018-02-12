using System.Collections.Generic;

namespace AutoWiki.Models
{
	internal static class LinkCache
	{
		private static readonly Dictionary<string, Link> Cache = new Dictionary<string, Link>();

		public static void SetLink(string key, string text, string page, string header)
		{
			Cache[key] = new Link
				{
					Text = text,
					Page = page,
					Header = header
				};
		}

		public static string GetLink(string text)
		{
			return Cache.TryGetValue(text, out var link) ? link.ToString() : text;
		}
	}

	internal class Link
	{
		public string Text { get; set; }
		public string Page { get; set; }
		public string Header { get; set; }

		public override string ToString()
		{
			return $"[{Text}]({Page}#{Header})";
		}
	}
}