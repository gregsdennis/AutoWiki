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
					switch (member.AssociatedMember)
					{
						case PropertyInfo property:
							_GenerateMarkdown(builder, property, member.Tags.OfType<ParamTag>().ToList());
							break;
						case FieldInfo field:
							_GenerateMarkdown(builder, field);
							break;
						case MethodInfo method:
							_GenerateMarkdown(builder, method, member.Tags);
							break;
						case ConstructorInfo constructor:
							_GenerateMarkdown(builder, constructor);
							break;
						case EventInfo @event:
							_GenerateMarkdown(builder, @event);
							break;
					}

					_GenerateMarkdown(builder, member.Tags.Where(t => !t.Handled));
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
				}
				tag.Handled = true;
				builder.Paragraph(tag.Text);
			}
		}

		private static void _GenerateMarkdown(StringBuilder builder, PropertyInfo property, IList<ParamTag> tags)
		{
			var indexes = property.GetIndexParameters();
			var name = $"{property.PropertyType.CSharpName()} {property.Name}";
			if (indexes.Any())
				name += $"[{string.Join(", ", indexes.Select(i => i.ParameterType.CSharpName()))}]";
			var getter = property.GetMethod;
			var setter = property.SetMethod;
			string get = null, set = null;
			if (getter?.IsPublic ?? false)
				get = "get; ";
			if (setter?.IsPublic ?? false)
				set = "set; ";
			name += $" {{ {get}{set}}}";

			builder.Header(3, name.AsCode());

			if (!indexes.Any()) return;

			foreach (var index in indexes)
			{
				_GenerateMarkdown(builder, index, tags);
			}
		}

		private static void _GenerateMarkdown(StringBuilder builder, MethodInfo method, IList<Tag> tags)
		{
			var name = $"{method.ReturnParameter.ParameterType.CSharpName()} {method.Name}({string.Join(", ", method.GetParameters().Select(p => p.ParameterType.CSharpName()))})";

			builder.Header(3, name.AsCode());
			var paramTags = tags.OfType<ParamTag>().ToList();

			foreach (var parameter in method.GetParameters())
			{
				_GenerateMarkdown(builder, parameter, paramTags);
			}

			var tag = tags.FirstOrDefault(t => t.Name == "returns");
			if (tag == null) return;

			tag.Handled = true;
			builder.Paragraph($"**Returns:** {tag.Text}");
		}

		private static void _GenerateMarkdown(StringBuilder builder, FieldInfo field)
		{
			builder.Header(3, field.Name.AsCode());
			if (!field.DeclaringType.IsEnum)
				builder.Paragraph($"**Type:** {field.FieldType.CSharpName().AsCode()}");
		}

		private static void _GenerateMarkdown(StringBuilder builder, EventInfo @event)
		{
			builder.Header(3, @event.Name.AsCode());
			builder.Paragraph($"**Handler Type:** {@event.EventHandlerType.CSharpName().AsCode()}");
		}

		private static void _GenerateMarkdown(StringBuilder builder, ConstructorInfo constructor)
		{
			builder.Header(3, constructor.Name.AsCode());
			//builder.Paragraph($"**Type:** {@event.con.CSharpName().AsCode()}");
		}

		private static void _GenerateMarkdown(StringBuilder builder, ParameterInfo parameter, IEnumerable<ParamTag> tags)
		{
			builder.Paragraph($"**Parameter:** {parameter.Name}");
			builder.Paragraph($"  **Type:** {parameter.ParameterType.CSharpName().AsCode()}");

			var tag = tags.FirstOrDefault(t => t.ParamName == parameter.Name);
			if (tag == null) return;

			tag.Handled = true;
			builder.Paragraph($"  {tag.Text}");
		}
	}
}