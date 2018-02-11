using System.Text;

namespace AutoWiki
{
	internal static class StringBuilderExtensions
	{
		public static void Header(this StringBuilder sb, int level, string content)
		{
			sb.AppendLine($"{new string('#', level)} {content}");
			sb.AppendLine();
		}

		public static void Paragraph(this StringBuilder sb, string content)
		{
			sb.AppendLine(content);
			sb.AppendLine();
		}
	}
}