using Comar.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;

namespace Comar.Serialization;

/// <summary> Wrapper around JSON serializer </summary>
internal sealed class JsonSerializer : ISerializer
{
	/// <inheritdoc />
	public T? Deserialize<T>(string contents)
	{
		try
		{
			return TryDeserialize<T>(contents);
		}
		catch (Exception e)
		{
			throw new SerializationException("Error occured during deserialization", e);
		}
	}
	
	private T? TryDeserialize<T>(string contents)
	{
		if (string.IsNullOrWhiteSpace(contents))
		{
			throw new SerializationException();
		}

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
