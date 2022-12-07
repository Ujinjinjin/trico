using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Comar.Serialization;

/// <summary> Wrapper around JSON serializer </summary>
internal sealed class JsonSerializer : SerializerBase
{
	protected override T? DeserializeObject<T>(string contents) where T : default
	{
		var settings = new JsonSerializerSettings
		{
			DefaultValueHandling = DefaultValueHandling.Populate,
			ContractResolver = new DefaultContractResolver
			{
				NamingStrategy = new KebabCaseNamingStrategy
				{
					OverrideSpecifiedNames = false
				}
			},
		};

		return JsonConvert.DeserializeObject<T>(contents, settings);
	}

	protected override string SerializeObject<T>(T data)
	{
		var settings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.Indented,
			ContractResolver = new DefaultContractResolver
			{
				NamingStrategy = new KebabCaseNamingStrategy
				{
					OverrideSpecifiedNames = false
				}
			},
		};

		return JsonConvert.SerializeObject(data, settings);
	}
}
