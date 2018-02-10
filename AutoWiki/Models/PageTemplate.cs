using System;
using System.Collections.Generic;
using Manatee.Json;
using Manatee.Json.Serialization;

namespace AutoWiki.Models
{
	internal class PageTemplate : IJsonSerializable
	{
		public List<TypeTemplate> Templates { get; set; }
		public string FileName { get; set; }

		public void FromJson(JsonValue json, JsonSerializer serializer)
		{
			if (json.Type != JsonValueType.Object)
				throw new Exception("A template must be a JSON object.");

			Templates = serializer.Deserialize<List<TypeTemplate>>(json.Object["sections"].Array);
		}

		public JsonValue ToJson(JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}