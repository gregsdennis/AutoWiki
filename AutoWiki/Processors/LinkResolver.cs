using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutoWiki.Models;

namespace AutoWiki.Processors
{
	internal static class LinkResolver
	{
		public static readonly Regex LinkPattern = new Regex(@"\[(?<name>.*?)]\(\)");
		public static readonly Regex NullablePattern = new Regex(@"(?<name>[^,]+)\?");
		public static readonly Regex TypeParameterPattern = new Regex(@"[<, ](?<name>[^,]+)[>,]");

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
					resolvedLink = _ResolvePartialTypes(NullablePattern, name, resolvedLink);
					resolvedLink = _ResolvePartialTypes(TypeParameterPattern, name, resolvedLink);
					link.Markdown = link.Markdown.Replace($"[{name}]()", resolvedLink);
				}
			}
		}

		private static string _ResolvePartialTypes(Regex pattern, string name, string resolvedLink)
		{
			var matches = pattern.Matches(name);
			if (matches.FirstOrDefault()?.Groups["name"].Success ?? false)
			{
				var nameComponent = matches[0].Groups["name"].Value;
				var componentLink = LinkCache.ResolveLink(nameComponent);
				if (nameComponent != componentLink)
					return resolvedLink.Replace(nameComponent, componentLink);
			}
			return resolvedLink;
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
