using Comar.Constants;
using Comar.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Comar.Serializers;

/// <summary> Wrapper around JSON serializer </summary>
internal sealed class JsonSerializer : ISerializer
{
	public string DefaultFileExtension => FilenameExtension.Json;

	/// <inheritdoc />
	public T? Deserialize<T>(string contents)
	{
		return System.Text.Json.JsonSerializer.Deserialize<T>(contents);
	}

	/// <inheritdoc />
	public string Serialize<T>(T data)
	{
		var options = new JsonSerializerOptions
		{
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			WriteIndented = true,
		};

		var serializedObject = System.Text.Json.JsonSerializer.Serialize(data, options);

		return serializedObject.ToUnixEol();
	}
}