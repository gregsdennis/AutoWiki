using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoWiki.Models;
using AutoWiki.Processors;
using Manatee.Json;
using Manatee.Json.Serialization;

namespace AutoWiki
{
	class Program
	{
		private const string _msdnStyleSwitch = "--msdn-style";
		private const string _gitHubPagesSwitch = "--gh-pages";

		static readonly string[] KnownSwitches =
		{
			_msdnStyleSwitch,
			_gitHubPagesSwitch
		};

		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				_Help();
				return;
			}

			var filePerType = args.Any(a => a == _msdnStyleSwitch);
			Link.UseGitHubPagesLinks = args.Any(a => a == _gitHubPagesSwitch);
			var wikiPath = args.FirstOrDefault(a => !KnownSwitches.Contains(a));

			if (string.IsNullOrWhiteSpace(wikiPath) || !Directory.Exists(wikiPath))
			{
				Console.WriteLine($"Could not find directory '{wikiPath}'");
				return;
			}

			var assemblyFiles = Directory.GetFiles(wikiPath, "*.dll");
			var commentFiles = Directory.GetFiles(wikiPath, "*.xml").ToList();

			var assemblies = assemblyFiles.Select(Assembly.LoadFrom).ToList();

			var templates = filePerType
				                ? _GetTypeBasedTemplates(wikiPath, assemblies)
				                : _GetTemplatesFromJsonFiles(wikiPath);

			var comments = commentFiles.SelectMany(XmlProcessor.Load).ToList();

			var pages = templates.Select(t => t.Generate(assemblies, comments)).ToList();

			LinkResolver.ValidateLinks();

			var markdownPages = pages.Select(p => new
				{
					Markdown = p.GenerateMarkdown(),
					File = p.FileName
				}).OrderBy(p => p.File).ToList();

			var fileTemplatePath = Path.Combine(wikiPath, "_template.md");
			var fileTemplate = File.Exists(fileTemplatePath)
				                   ? File.ReadAllText(fileTemplatePath)
				                   : null;

			int order = 1;
			foreach (var page in markdownPages)
			{
				var markdown = page.Markdown.ResolveLinks();

				if (fileTemplate != null)
				{
					markdown = fileTemplate.Replace("{{content}}", markdown)
					                       .Replace("{{filename}}", Path.GetFileNameWithoutExtension(page.File).Replace("`1", "<T>"))
					                       .Replace("{{order}}", order.ToString());
				}

				markdown = markdown.EscapeForMarkdown();

				File.WriteAllText(page.File, markdown);

				order++;
			}
		}

		private static IEnumerable<PageTemplate> _GetTemplatesFromJsonFiles(string wikiPath)
		{
			var jsonFiles = Directory.GetFiles(wikiPath, "*.json");
			var serializer = new JsonSerializer();

			var templates = jsonFiles.Select(x =>
				{
					var template = serializer.Deserialize<PageTemplate>(JsonValue.Parse(File.ReadAllText(x)));
					template.FileName = x.ChangeExtension("md");
					return template;
				});
			return templates;
		}

		private static IEnumerable<PageTemplate> _GetTypeBasedTemplates(string wikiPath, IEnumerable<Assembly> assemblies)
		{
			return assemblies.SelectMany(a => a.DefinedTypes)
			                 .Where(t => t.IsPublic)
			                 .Select(t =>
				                 {
					                 var template = new PageTemplate
						                 {
							                 FileName = Path.Combine(wikiPath, $"{t.Name}.md"),
											 Templates = new List<TypeTemplate>
												 {
													 new TypeTemplate
														 {
															 Assembly = t.Assembly.GetName().Name,
															 Type = t.FullName
														 }
												 }
						                 };

					                 return template;
				                 });
		}

		private static void _Help()
		{
			Console.WriteLine("AutoWiki is a .Net documentation application that generates wiki files " +
			                  "in markdown based on the XML documentation present in the code.");
			Console.WriteLine();
			Console.WriteLine("Requirements can be found at the project wiki:");
			Console.WriteLine();
			Console.WriteLine("    http://github.com/gregsdennis/autowiki/wiki/");
			Console.WriteLine();
			Console.WriteLine("Usage: AutoWiki <folder>");
			Console.WriteLine();
			Console.WriteLine("  folder - the a folder containing compiled DLLs, XMLs, and JSON templates");
			Console.WriteLine();
		}
	}
}
