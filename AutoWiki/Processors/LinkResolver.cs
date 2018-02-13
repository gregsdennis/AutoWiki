using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutoWiki.Models;

namespace AutoWiki.Processors
{
	internal static class LinkResolver
	{
		public static readonly Regex LinkPattern = new Regex(@"\[(?<name>.*?)]\(\)");
		public static readonly Regex NullablePattern = new Regex(@"Nullable`1\[\[(?<name>[^,]+)");

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
					var nullableMatches = NullablePattern.Matches(name);
					if (nullableMatches.FirstOrDefault()?.Groups["name"].Success ?? false)
					{
						var nullableName = nullableMatches.FirstOrDefault().Groups["name"].Value;
						var nullableLink = LinkCache.ResolveLink(nullableName);
						if (nullableName != nullableLink)
							resolvedLink = $"{nullableLink}?";
					}
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
