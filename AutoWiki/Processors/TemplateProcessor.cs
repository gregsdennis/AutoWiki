using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoWiki.Models;
using Type = AutoWiki.Models.Type;

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
				page.Types.Add(_GenerateTypeDocumentation(type, comments));
			}

			return page;
		}

		private static Type _GenerateTypeDocumentation(TypeInfo type, List<XmlDocumentation> comments)
		{
			return new Type
				{
					Assembly = type.Assembly.FullName,
					AssociatedType = type.AsType(),
					Name = type.Name,
					Namespace = type.Namespace
				};
		}
	}
}