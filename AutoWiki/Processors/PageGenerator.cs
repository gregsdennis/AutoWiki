using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoWiki.Models;
using Humanizer;

namespace AutoWiki.Processors
{
	internal static class PageGenerator
	{
		private static readonly MemberType[] SortByKeys = new[]
			{
				MemberType.Constructor,
				MemberType.Field,
				MemberType.Property,
				MemberType.Event,
				MemberType.Method
			};

		public static string GenerateMarkdown(this Page page)
		{
			var builder = new StringBuilder();

			foreach (var typeDoc in page.Types)
			{
				_GenerateMarkdown(builder, typeDoc);
			}

			return builder.ToString();
		}

		private static void _GenerateMarkdown(StringBuilder builder, TypeDoc typeDoc)
		{
			builder.Header(1, typeDoc.AssociatedType.CSharpName().AsCode());

			_GenerateMarkdown(builder, typeDoc.Tags);

			var sortedMembers = typeDoc.Members.GroupBy(m => m.MemberType)
			                           .SortBy(SortByKeys, g => g.Key);

			foreach (var docGroup in sortedMembers)
			{
				if (!docGroup.Any()) continue;

				builder.Header(2, docGroup.Key.ToString().Pluralize());

				foreach (var member in docGroup)
				{
					builder.Header(3, member.AssociatedMember.Name.AsCode());
					builder.AppendLine();

					_GenerateMarkdown(builder, member.AssociatedMember as PropertyInfo);

					_GenerateMarkdown(builder, member.Tags);
					builder.AppendLine();
				}
			}
		}

		private static void _GenerateMarkdown(StringBuilder builder, IEnumerable<Tag> tags)
		{
			foreach (var tag in tags)
			{
				if (tag.Name != "summary")
				{
					builder.Header(3, tag.Name.Pascalize());
					builder.AppendLine();
				}
				builder.AppendLine(tag.Text);
				builder.AppendLine();
			}
		}

		private static void _GenerateMarkdown(StringBuilder builder, PropertyInfo property)
		{
			if (property == null) return;

			builder.AppendLine($"**Type:** {property.PropertyType.CSharpName().AsCode()}");
			builder.AppendLine();
		}
	}
}