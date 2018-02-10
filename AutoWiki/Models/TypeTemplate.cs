using System;
using Manatee.Json;
using Manatee.Json.Serialization;

namespace AutoWiki.Models
{
	internal class TypeTemplate : IJsonSerializable
	{
		public string Assembly { get; set; }
		public string Type { get; set; }

		public void FromJson(JsonValue json, JsonSerializer serializer)
		{
			if (json.Type != JsonValueType.Object)
				throw new Exception("A template must be a JSON object.");

			Assembly = json.Object["assembly"].String;
			// TODO: account for generics
			Type = json.Object["type"].String;
		}

		public JsonValue ToJson(JsonSerializer serializer)
		{
			throw new System.NotImplementedException();
		}
	}
}