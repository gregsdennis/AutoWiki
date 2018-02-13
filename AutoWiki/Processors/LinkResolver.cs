using System;
using System.Text.RegularExpressions;
using AutoWiki.Models;

namespace AutoWiki.Processors
{
	internal static class LinkResolver
	{
		public static readonly Regex LinkPattern = new Regex(@"\[(?<name>.*?)]\(\)");

		public static void ValidateLinks()
		{
			foreach (var cachedLink in LinkCache.Cache.Keys)
			{
				var link = LinkCache.Cache[cachedLink];
				var matches = LinkPattern.Matches(link.Markdown);
				foreach (Match match in matches)
				{
					var name = match.Groups["name"].Value;
					Type type;
					try
					{
						type = Type.GetType(name);
					}
					catch
					{
						type = null;
					}
					var resolvedLink = LinkCache.ResolveLink(type?.CSharpName() ?? name);
					link.Markdown = link.Markdown.Replace($"[{name}]()", resolvedLink);
				}
			}
		}

		public static string ResolveLinks(this string page)
		{
			var links = LinkPattern.Matches(page);
			foreach (Match match in links)
			{
				var name = match.Groups["name"].Value;
				var link = LinkCache.ResolveLink(name);
				page = page.Replace($"[{name}]()", link);
			}

			return page;
		}
	}
}
