using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using AutoWiki.Models;

namespace AutoWiki.Processors
{
	internal static class TemplateProcessor
	{
		public static Page Generate(this PageTemplate template, List<Assembly> assemblies, List<XmlDocumentation> docs)
		{
			var page = new Page {FileName = template.FileName};

			foreach (var typeTemplate in template.Templates)
			{
				var assembly = assemblies.FirstOrDefault(a => a.GetName().Name == typeTemplate.Assembly);
				var comments = docs.Where(d => d.AssemblyName == typeTemplate.Assembly &&
											   d.MemberName.StartsWith(typeTemplate.Type))
								   .ToList();
				var type = assembly.DefinedTypes.FirstOrDefault(t => t.FullName == typeTemplate.Type);
				if (type == null)
					throw new ArgumentException($"Could not find type '{typeTemplate.Type}'");
				page.Types.Add(_GenerateTypeDoc(type, comments, Path.GetFileNameWithoutExtension(template.FileName)));
			}

			return page;
		}

		private static TypeDoc _GenerateTypeDoc(TypeInfo typeInfo, List<XmlDocumentation> comments, string fileName)
		{
			var link = LinkCache.SetLink(typeInfo.FullName, typeInfo.Name, fileName);
			link.Markdown = typeInfo.Name;

			var doc = new TypeDoc {AssociatedType = typeInfo.AsType()};

			var typeComment = comments.FirstOrDefault(c => c.MemberName == typeInfo.FullName &&
														   c.MemberType == MemberType.Type);
			doc.Tags.AddRange(typeComment.Data.Select(_ConvertToTag));

			var memberComments = typeInfo.GetAllMembers()
										 .Where(m => m.IsPublic())
										 .Join(comments,
											   m => _GetMemberName(typeInfo, m),
											   c => c.MemberName,
											   (m, c) => new {Member = m, Comment = c});

			doc.Members.AddRange(memberComments.Select(mc => _ConvertToMember(mc.Member, mc.Comment, fileName)));

			return doc;
		}
		private static string _GetMemberName(TypeInfo typeInfo, MemberInfo member)
		{
			if (member is MethodBase method)
			{
				var name = method is ConstructorInfo ? "#ctor" : method.Name;
				var parameters = method.GetParameters();
				var formattableString = parameters.Length == 0
					                           ? $"{typeInfo.FullName}.{name}"
					                           : $"{typeInfo.FullName}.{name}({string.Join(",", method.GetParameters().Select(p => p.ParameterType.FullName))})";
				return formattableString;
			}
			else if (member is PropertyInfo property)
			{
				var name = property.Name;
				var formattableString = $"{typeInfo.FullName}.{name}({string.Join(",", property.GetIndexParameters().Select(p => p.ParameterType.FullName))})";
				return formattableString;
			}

			return $"{typeInfo.FullName}.{member.Name}";
		}

		private static MemberDoc _ConvertToMember(MemberInfo memberInfo, XmlDocumentation member, string fileName)
		{
			var link = LinkCache.SetLink(memberInfo.GetLinkKey(),
										 (memberInfo.IsStatic() ?? false) ? $"{memberInfo.DeclaringType.Name}.{memberInfo.Name}" : memberInfo.Name,
										 fileName);
			MarkdownGenerator.UpdateLinkForMember(link, memberInfo);

			var doc = new MemberDoc
				{
					AssociatedMember = memberInfo,
					MemberType = member.MemberType
				};

			doc.Tags.AddRange(member.Data.Select(_ConvertToTag));

			return doc;
		}

		private static Tag _ConvertToTag(XElement xml)
		{
			Tag tag;
			switch (xml.Name.LocalName)
			{
				case "typeparam":
					tag = _ConvertToTypeParamTag(xml);
					break;
				case "param":
					tag = _ConvertToParamTag(xml);
					break;
				case "exception":
					tag = _ConvertToExceptionTag(xml);
					break;
				default:
					tag = _ConvertToSimpleTag(xml);
					break;
			}

			tag.Text = string.Concat(xml.Nodes().Select(_ConvertNode)).Trim();

			return tag;
		}

		private static Tag _ConvertToTypeParamTag(XElement xml)
		{
			var tag = new TypeParamTag
				{
					Name = "typeparam",
					ParamName = xml.Attribute("name").Value
				};

			return tag;
		}

		private static Tag _ConvertToParamTag(XElement xml)
		{
			var tag = new ParamTag
				{
					Name = "param",
					ParamName = xml.Attribute("name").Value
				};

			return tag;
		}

		private static Tag _ConvertToExceptionTag(XElement xml)
		{
			var details = xml.GetMemberDetails("cref", new XmlDocumentation());
			var tag = new ExceptionTag
				{
					Name = "param",
					ExceptionType = details.MemberName
				};

			return tag;
		}

		private static Tag _ConvertToSimpleTag(XElement xml)
		{
			var tag = new Tag
				{
					Name = xml.Name.LocalName
				};

			return tag;
		}

		/// <summary>
		/// </summary>
		/// <param name="xNode"></param>
		/// <returns></returns>
		private static string _ConvertNode(XNode xNode)
		{
			switch (xNode)
			{
				case XElement element:
					switch (element.Name.LocalName)
					{
						case "see":
							var xml = element.GetMemberDetails("cref", new XmlDocumentation());
							return $"[{xml.MemberName}]()";
						case "paramref":
						case "typeparamref":
							var attr = element.Attribute("name").Value;
							return $"*{attr}*";
						// TODO: check for things like <b> for formatting
						default:
							return element.ToString();
					}
				case XText text:
					return $"{text.Value}";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}