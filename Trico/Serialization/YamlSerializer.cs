using Trico.Extensions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Trico.Serialization;

/// <summary> Wrapper around YAML serializer </summary>
internal sealed class YamlSerializer : SerializerBase
{
	protected override T DeserializeObject<T>(string contents) where T : default
	{
		var serializer = new DeserializerBuilder()
			.WithNamingConvention(HyphenatedNamingConvention.Instance)
			.IgnoreUnmatchedProperties()
			.Build();

		return serializer.Deserialize<T>(contents);
	}

	/// <inheritdoc />
	protected override string SerializeObject<T>(T data)
	{
		var serializer = new SerializerBuilder()
			.WithNamingConvention(HyphenatedNamingConvention.Instance)
			.ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
			.Build();

		return serializer.Serialize(data).ToUnixEol();
	}
}
