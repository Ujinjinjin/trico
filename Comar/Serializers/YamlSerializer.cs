using Comar.Constants;
using Comar.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Comar.Serializers;

/// <summary> Wrapper around YAML serializer </summary>
internal sealed class YamlSerializer : ISerializer
{
	public string DefaultFileExtension => FilenameExtension.Yml;

	/// <inheritdoc />
	public T? Deserialize<T>(string contents)
	{
		var serializer = new DeserializerBuilder()
			.WithNamingConvention(HyphenatedNamingConvention.Instance)
			.IgnoreUnmatchedProperties()
			.Build();

		return serializer.Deserialize<T>(contents);
	}

	/// <inheritdoc />
	public string Serialize<T>(T data)
	{
		var serializer = new SerializerBuilder()
			.WithNamingConvention(HyphenatedNamingConvention.Instance)
			.ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
			.Build();

		if (data is null)
		{
			return string.Empty;
		}

		return serializer.Serialize(data).ToUnixEol();
	}
}
