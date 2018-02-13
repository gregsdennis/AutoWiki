using System.Collections.Generic;

namespace AutoWiki.Models
{
	internal static class LinkCache
	{
		public static readonly Dictionary<string, Link> Cache = new Dictionary<string, Link>();

		public static Link SetLink(string key, string text, string page)
		{
			var link = new Link
				{
					Text = text,
					Page = page
				};
			Cache[key] = link;

			return link;
		}

		public static Link GetLink(string key)
		{
			return Cache.TryGetValue(key, out var link) ? link : null;
		}

		public static string ResolveLink(string key, string alternateText = null)
		{
			return Cache.TryGetValue(key, out var link) ? link.ToString() : alternateText ?? key;
		}
	}
}