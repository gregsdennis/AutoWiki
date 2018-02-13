using System;
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
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				_Help();
				return;
			}

			if (!Directory.Exists(args[0]))
			{
				Console.WriteLine($"Could not find directory '{args[0]}'");
				return;
			}

			var jsonFiles = Directory.GetFiles(args[0], "*.json");
			var assemblyFiles = Directory.GetFiles(args[0], "*.dll");
			var commentFiles = Directory.GetFiles(args[0], "*.xml").ToList();

			var serializer = new JsonSerializer();
			var templates = jsonFiles.Select(x =>
				{
					var template = serializer.Deserialize<PageTemplate>(JsonValue.Parse(File.ReadAllText(x)));
					template.FileName = x.ChangeExtension("md");
					return template;
				});

			var assemblies = assemblyFiles.Select(Assembly.LoadFrom).ToList();

			var comments = commentFiles.SelectMany(XmlProcessor.Load).ToList();

			var pages = templates.Select(t => t.Generate(assemblies, comments)).ToList();

			LinkResolver.ValidateLinks();

			var markdownPages = pages.Select(p => new
				{
					Markdown = p.GenerateMarkdown(),
					File = p.FileName
				}).ToList();

			foreach (var page in markdownPages)
			{
				var markdown = page.Markdown.ResolveLinks();
				File.WriteAllText(page.File, markdown);
			}
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
