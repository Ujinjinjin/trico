using Comar.Constants;
using Comar.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Comar.Serializers;

/// <summary> Wrapper around JSON serializer </summary>
internal sealed class JsonSerializer : ISerializer
{
	public string DefaultFileExtension => FilenameExtension.Json;

	/// <inheritdoc />
	public T? Deserialize<T>(string contents)
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

	/// <inheritdoc />
	public string Serialize<T>(T data)
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

		var serializedObject = JsonConvert.SerializeObject(data, settings);

		return serializedObject.ToUnixEol();
	}
}
