using System.Text.RegularExpressions;
using AutoWiki.Models;

namespace AutoWiki.Processors
{
	internal static class LinkResolver
	{
		private static readonly Regex LinkPattern = new Regex(@"\[(?<name>[\._a-zA-Z0-9]*)]\((?<type>.)*?\)");

		public static string ResolveLinks(this string page)
		{
			var links = LinkPattern.Matches(page);
			foreach (Match match in links)
			{
				var name = match.Groups["type"].Success
					           ? match.Groups["type"].Value
					           : match.Groups["name"].Value;
				var link = LinkCache.GetLink(name);
				page = Regex.Replace(page, $@"\[{name}]\((.+)?\)", link);
			}

			return page;
		}
	}
}
