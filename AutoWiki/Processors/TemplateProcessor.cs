using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using AutoWiki.Models;

namespace AutoWiki.Processors
{
	internal static class TemplateProcessor
	{
		public static Page Generate(this PageTemplate template, List<Assembly> assemblies, List<XmlDocumentation> docs)
		{
			var page = new Page();

			page.FileName = template.FileName;

			foreach (var typeTemplate in template.Templates)
			{
				var assembly = assemblies.FirstOrDefault(a => a.GetName().Name == typeTemplate.Assembly);
				var comments = docs.Where(d => d.AssemblyName == typeTemplate.Assembly &&
				                               d.MemberName.StartsWith(typeTemplate.Type))
				                   .ToList();
				var type = assembly.DefinedTypes.FirstOrDefault(t => t.FullName == typeTemplate.Type);
				page.Types.Add(_GenerateTypeDoc(type, comments));
			}

			return page;
		}

		private static TypeDoc _GenerateTypeDoc(TypeInfo typeInfo, List<XmlDocumentation> comments)
		{
			var doc = new TypeDoc {AssociatedType = typeInfo.AsType()};

			var typeComment = comments.FirstOrDefault(c => c.MemberName == typeInfo.FullName &&
			                                               c.MemberType == MemberType.Type);
			doc.Tags.AddRange(typeComment.Data.Select(_ConvertToTag));

			var memberComments = typeInfo.DeclaredMembers.Join(comments,
			                                                   m => $"{typeInfo.FullName}.{m.Name}",
			                                                   c => c.MemberName,
			                                                   (m, c) => new {Member = m, Comment = c});

			doc.Members.AddRange(memberComments.Select(mc => _ConvertToMember(mc.Member, mc.Comment)));

			return doc;
		}

		private static MemberDoc _ConvertToMember(MemberInfo memberInfo, XmlDocumentation member)
		{
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
			var tag = new ExceptionTag
				{
					Name = "param",
					ExceptionType = xml.Attribute("cref").Value
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
					var xml = new XmlDocumentation();
					switch (element.Name.LocalName)
					{
						case "see":
							element.GetMemberDetails("cref", xml);
							return $"[{xml.MemberName}]()";
						case "paramref":
						case "typeparamref":
							element.GetMemberDetails("name", xml);
							return $"[{xml.MemberName}]()";
						// TODO: check for things like <b> for formatting
						default:
							throw new NotImplementedException();
					}
				case XText text:
					return $"{text.Value}";
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}