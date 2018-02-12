using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AutoWiki.Models;

namespace AutoWiki
{
	internal static class StringBuilderExtensions
	{
		public static void Header(this StringBuilder sb, int level, string content, string pageName = null, string memberName = null)
		{
			if (pageName != null)
				LinkCache.SetLink($"{memberName}", content, pageName, _ConvertToLink(content));

			sb.AppendLine($"{new string('#', level)} {content}");
			sb.AppendLine();
		}

		public static void Paragraph(this StringBuilder sb, string content)
		{
			sb.AppendLine(content);
			sb.AppendLine();
		}

		private static string _ConvertToLink(string content)
		{
			var link = Regex.Replace(content, @"[^\w]", "-");
			link = Regex.Replace(link, "-+", "-");

			return link.ToLower();
		}
	}
}