using System.Text;
using System.Text.RegularExpressions;
using AutoWiki.Models;

namespace AutoWiki
{
	internal static class StringBuilderExtensions
	{
		public static void Header(this StringBuilder sb, int level, string header, string pageName = null, string memberName = null, string linkText = null)
		{
			if (pageName != null)
				LinkCache.SetLink(memberName, linkText ?? header, pageName, _ConvertToLink(header));

			sb.AppendLine(Regex.Replace($"{new string('#', level)} {header}", @"\s+", " ", RegexOptions.Multiline));
			sb.AppendLine();
		}

		public static void Paragraph(this StringBuilder sb, string content)
		{
			sb.AppendLine(Regex.Replace(content, @"\s+", " ", RegexOptions.Multiline));
			sb.AppendLine();
		}

		private static string _ConvertToLink(string content)
		{
			var link = Regex.Replace(content, @"[^\w\s]", string.Empty);
			link = Regex.Replace(link, @"\s", "-");

			return link.ToLower();
		}
	}
}