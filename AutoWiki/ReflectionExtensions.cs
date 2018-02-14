using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AutoWiki
{
	internal static class ReflectionExtensions
	{
		private static readonly Dictionary<Type, string> KeywordedTypes = 
			new Dictionary<Type, string>
				{
					[typeof(void)] = "void",
					[typeof(string)] = "string",
					[typeof(int)] = "int",
					[typeof(short)] = "short",
					[typeof(long)] = "long",
					[typeof(ulong)] = "ulong",
					[typeof(uint)] = "uint",
					[typeof(ushort)] = "ushort",
					[typeof(double)] = "double",
					[typeof(float)] = "float",
					[typeof(byte)] = "byte",
					[typeof(char)] = "char",
					[typeof(bool)] = "bool",
				};

		public static string CSharpName(this Type type)
		{
			if (KeywordedTypes.TryGetValue(type, out var keyword)) return keyword;

			var sb = new StringBuilder();
			var name = type.Name;
			if (!type.IsGenericType) return name;

			if (type.GetGenericTypeDefinition() == typeof(Nullable<>)) return $"{CSharpName(type.GetGenericArguments()[0])}?";

			sb.Append(name.Substring(0, name.IndexOf('`')));
			sb.Append("<");
			sb.Append(string.Join(", ", type.GetGenericArguments()
			                                .Select(CSharpName)));
			sb.Append(">");
			return sb.ToString();
		}

		public static bool IsPublic(this MemberInfo member)
		{
			return (member as FieldInfo)?.IsPublic ??
			       (member as PropertyInfo)?.GetAccessors().Any(a => a.IsPublic) ??
			       (member as MethodInfo)?.IsPublic ??
			       (member as ConstructorInfo)?.IsPublic ??
			       _IsPublic(member as EventInfo) ??
			       false;
		}

		public static IEnumerable<MemberInfo> GetAllMembers(this TypeInfo type)
		{
			if (type == null) return Enumerable.Empty<MemberInfo>();

			if (type.BaseType != null)
				return type.DeclaredMembers.Union(type.BaseType.GetTypeInfo().DeclaredMembers)
				           .Distinct(MemberInfoComparer.Instance);

			return type.DeclaredMembers;
		}

		private static bool? _IsPublic(EventInfo @event)
		{
			if (@event == null) return null;

			return @event.AddMethod.IsPublic || @event.RemoveMethod.IsPublic;
		}

		public static bool? IsStatic(this MemberInfo member)
		{
			return (member as FieldInfo)?.IsStatic ??
			       (member as PropertyInfo)?.GetMethod?.IsStatic ??
			       (member as PropertyInfo)?.SetMethod?.IsStatic ??
			       (member as MethodBase)?.IsStatic ??
			       (member as EventInfo)?.AddMethod?.IsStatic ??
			       false;
		}

		public static string GetParameterList(this MemberInfo member)
		{
			switch (member)
			{
				case MethodBase method:
					return $"({string.Join(", ", method.GetParameters().Select(p => $"{p.ParameterType.CSharpName()} {p.Name}"))})";
				case PropertyInfo property:
					var indexes = property.GetIndexParameters();
					if (indexes.Any())
						return $"[{string.Join(", ", indexes.Select(p => $"{p.ParameterType.CSharpName()} {p.Name}"))}]";
					return null;
				default:
					return null;
			}
		}

		public static string GetLinkKey(this MemberInfo member)
		{
			return $"{member.DeclaringType?.FullName}.{member.Name}{member.GetParameterList()}";
		}

		public static string AsLinkRequest(this Type type)
		{
			return $"[{type.FullName}]()";
		}
	}
}