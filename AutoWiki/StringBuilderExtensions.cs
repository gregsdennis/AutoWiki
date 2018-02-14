using System.Text;
using System.Text.RegularExpressions;

namespace AutoWiki
{
	internal static class StringBuilderExtensions
	{
		public static void Header(this StringBuilder sb, int level, string header)
		{
			sb.AppendLine(Regex.Replace($"{new string('#', level)} {header}", @"[^\S\n]+", " "));
			sb.AppendLine();
		}

		public static void Paragraph(this StringBuilder sb, string content)
		{
			var text = Regex.Replace(content, @"[^\S\n]+", " ");
			text = Regex.Replace(text, @"\n[^\S\n]+", "\n");
			sb.AppendLine(text);
			sb.AppendLine();
		}
	}
}