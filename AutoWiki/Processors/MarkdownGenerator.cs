using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoWiki.Models;
using Humanizer;

namespace AutoWiki.Processors
{
	internal static class MarkdownGenerator
	{
		private static readonly MemberType[] SortByKeys =
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

			return builder.ToString().AddTableOfContents();
		}

		public static void UpdateLinkForMember(Link link, MemberInfo member)
		{
			switch (member)
			{
				case PropertyInfo property:
					_GenerateMarkdownForLink(link, property);
					break;
				case FieldInfo field:
					_GenerateMarkdownForLink(link, field);
					break;
				case MethodInfo method:
					_GenerateMarkdownForLink(link, method);
					break;
				case ConstructorInfo constructor:
					_GenerateMarkdownForLink(link, constructor);
					break;
				case EventInfo @event:
					_GenerateMarkdownForLink(link, @event);
					break;
			}
		}

		private static void _GenerateMarkdown(StringBuilder builder, TypeDoc typeDoc)
		{
			builder.Header(1, typeDoc.AssociatedType.CSharpName());
			if (typeDoc.AssociatedType.IsGenericType)
			{
				var generic = typeDoc.AssociatedType.GetGenericTypeDefinition();
				var typeParamTags = typeDoc.Tags.OfType<TypeParamTag>().ToList();
				foreach (var parameter in generic.GetTypeInfo().GenericTypeParameters)
				{
					_GenerateMarkdown(builder, parameter, typeParamTags);
				}
			}

			_GenerateMarkdown(builder, typeDoc.Tags, "summary");

			builder.Paragraph($"**Assembly:** {Path.GetFileName(typeDoc.AssociatedType.Assembly.Location)}");

			// TODO : list base type & implementations

			var currentType = typeDoc.AssociatedType;
			var list = new List<string>();
			while (currentType != null)
			{
				list.Add($"- {currentType.CSharpName()}");
				currentType = currentType.BaseType;
			}
			list.Reverse();
			builder.Paragraph($"**Inheritance hierarchy:**");
			foreach (var baseClass in list)
			{
				builder.AppendLine(baseClass);
			}
			builder.AppendLine();

			_GenerateMarkdown(builder, typeDoc.Tags);

			var sortedMembers = typeDoc.Members
			                           .OrderBy(m => !m.AssociatedMember.IsStatic())
			                           .ThenBy(m => m.AssociatedMember.Name)
			                           .GroupBy(m => m.MemberType)
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
							_GenerateMarkdown(builder, property, member.Tags.ToList());
							break;
						case FieldInfo field:
							_GenerateMarkdown(builder, field, member.Tags);
							break;
						case MethodInfo method:
							_GenerateMarkdown(builder, method, member.Tags);
							break;
						case ConstructorInfo constructor:
							_GenerateMarkdown(builder, constructor, member.Tags);
							break;
						case EventInfo @event:
							_GenerateMarkdown(builder, @event, member.Tags);
							break;
					}

					_GenerateMarkdown(builder, member.Tags.Where(t => !t.Handled));
				}
			}
		}

		private static void _GenerateMarkdown(StringBuilder builder, IEnumerable<Tag> tags, string tagName)
		{
			var tag = tags.FirstOrDefault(t => t.Name == tagName);

			if (tag == null) return;

			tag.Handled = true;
			builder.Paragraph(tag.Text);
		}

		private static void _GenerateMarkdown(StringBuilder builder, IEnumerable<Tag> tags)
		{
			foreach (var tag in tags.Where(t => !t.Handled))
			{
				builder.Header(4, tag.Name.Replace("-", " ").Titleize());
				tag.Handled = true;
				builder.Paragraph(tag.Text);
			}
		}

		private static void _GenerateMarkdown(StringBuilder builder, PropertyInfo property, IList<Tag> tags)
		{
			var link = LinkCache.GetLink(property.GetLinkKey());
			builder.Header(3, link.Markdown);

			_GenerateMarkdown(builder, tags, "summary");

			var indexes = property.GetIndexParameters();
			if (!indexes.Any()) return;

			foreach (var index in indexes)
			{
				_GenerateMarkdown(builder, index, tags.OfType<ParamTag>());
			}
		}

		private static void _GenerateMarkdownForLink(Link link, PropertyInfo property)
		{
			var markdown = $"{property.PropertyType.AsLinkRequest()} {property.Name}{property.GetParameterList()}";
			var getter = property.GetMethod;
			var setter = property.SetMethod;
			string get = null, set = null;
			if (getter?.IsPublic ?? false)
				get = "get; ";
			if (setter?.IsPublic ?? false)
				set = "set; ";
			markdown += $" {{ {get}{set}}}";
			var isStatic = (getter?.IsStatic ?? false) || (setter?.IsStatic ?? false);
			if (isStatic)
				markdown = $"static {markdown}";

			var linkText = isStatic ? $"{property.DeclaringType.Name}.{property.Name}" : property.Name;

			link.Text = linkText;
			link.Markdown = markdown;
		}

		private static void _GenerateMarkdown(StringBuilder builder, MethodInfo method, IList<Tag> tags)
		{
			var link = LinkCache.GetLink(method.GetLinkKey());
			builder.Header(3, link.Markdown);

			_GenerateMarkdown(builder, tags, "summary");

			if (method.IsGenericMethod)
			{
				var generic = method.GetGenericMethodDefinition();
				var typeParamTags = tags.OfType<TypeParamTag>().ToList();
				foreach (var parameter in generic.GetGenericArguments())
				{
					_GenerateMarkdown(builder, parameter, typeParamTags);
				}
			}
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

		private static void _GenerateMarkdownForLink(Link link, MethodInfo method)
		{
			var markdown = $"{method.ReturnType.AsLinkRequest()} {method.Name}{method.GetParameterList()}";
			if (method.IsStatic)
				markdown = $"static {markdown}";

			var linkText = method.IsStatic ? $"{method.DeclaringType.Name}.{method.Name}" : method.Name;

			link.Text = linkText;
			link.Markdown = markdown;
		}

		private static void _GenerateMarkdown(StringBuilder builder, FieldInfo field, IList<Tag> tags)
		{
			var link = LinkCache.GetLink(field.GetLinkKey());
			builder.Header(3, link.Markdown);

			_GenerateMarkdown(builder, tags, "summary");
		}

		private static void _GenerateMarkdownForLink(Link link, FieldInfo field)
		{
			var isEnum = field.DeclaringType.IsEnum;
			var type = isEnum ? null : $"{field.FieldType.AsLinkRequest()} ";
			var markdown = $"{type}{field.Name}";
			if (field.IsStatic && !isEnum)
				markdown = $"static {markdown}";

			var linkText = field.IsStatic ? $"{field.DeclaringType.Name}.{field.Name}" : field.Name;

			link.Text = linkText;
			link.Markdown = markdown;
		}

		private static void _GenerateMarkdown(StringBuilder builder, EventInfo @event, IList<Tag> tags)
		{
			var link = LinkCache.GetLink(@event.GetLinkKey());
			builder.Header(3, link.Markdown);

			_GenerateMarkdown(builder, tags, "summary");
		}

		private static void _GenerateMarkdownForLink(Link link, EventInfo @event)
		{
			var markdown = $"{@event.EventHandlerType.AsLinkRequest()} {@event.Name}";
			if (@event.AddMethod.IsStatic)
				markdown = $"static {markdown}";

			var linkText = @event.AddMethod.IsStatic ? $"{@event.DeclaringType.Name}.{@event.Name}" : @event.Name;

			link.Text = linkText;
			link.Markdown = markdown;
		}

		private static void _GenerateMarkdown(StringBuilder builder, ConstructorInfo constructor, List<Tag> tags)
		{
			var link = LinkCache.GetLink(constructor.GetLinkKey());
			builder.Header(3, link.Markdown);

			_GenerateMarkdown(builder, tags, "summary");

			var paramTags = tags.OfType<ParamTag>().ToList();
			foreach (var parameter in constructor.GetParameters())
			{
				_GenerateMarkdown(builder, parameter, paramTags);
			}
		}

		private static void _GenerateMarkdownForLink(Link link, ConstructorInfo method)
		{
			var markdown = $"{method.DeclaringType.CSharpName()}{method.GetParameterList()}";
			if (method.IsStatic)
				markdown = $"static {markdown}";

			var linkText = method.IsStatic ? $"{method.DeclaringType.Name}.{method.Name}" : method.Name;

			link.Text = linkText;
			link.Markdown = markdown;
		}

		private static void _GenerateMarkdown(StringBuilder builder, ParameterInfo parameter, IEnumerable<ParamTag> tags)
		{
			builder.Paragraph($"**Parameter:** {parameter.Name}");

			var tag = tags.FirstOrDefault(t => t.ParamName == parameter.Name);
			if (tag == null) return;

			tag.Handled = true;
			builder.Paragraph($"{tag.Text}");
		}

		private static void _GenerateMarkdown(StringBuilder builder, Type parameter, IEnumerable<TypeParamTag> tags)
		{
			var text = $"**Type Parameter:** {parameter.Name}";

			var constraints = parameter.GetGenericParameterConstraints();
			var attributes = parameter.GenericParameterAttributes;
			var constraintList = new List<string>();
			if (attributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
				constraintList.Add("class");
			else if (attributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
				constraintList.Add("struct");
			constraintList.AddRange(constraints.OrderBy(c => c.IsClass)
			                                   .Select(constraint => constraint.AsLinkRequest()));
			if (attributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
				constraintList.Add("new()");
			if (constraintList.Any())
			{
				text += $" : {string.Join(", ", constraintList)}";
			}
			builder.Paragraph(text);

			var tag = tags.FirstOrDefault(t => t.ParamName == parameter.Name);
			if (tag == null) return;

			tag.Handled = true;
			builder.Paragraph($"{tag.Text}");
		}
	}
}