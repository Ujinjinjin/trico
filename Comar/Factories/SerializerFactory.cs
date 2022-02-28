using Comar.Constants;
using Comar.Serializers;

namespace Comar.Factories;

/// <inheritdoc />
public sealed class SerializerFactory : ISerializerFactory
{
	/// <inheritdoc />
	public ISerializer CreateSerializer(string filepath)
	{
		var fileExtension = filepath.Split('.')[^1];

		switch (fileExtension)
		{
			case FilenameExtension.Json:
				return new JsonSerializer();
			case FilenameExtension.Yaml:
			case FilenameExtension.Yml:
				return new YamlSerializer();
			default:
				throw new ArgumentOutOfRangeException(filepath);
		}
	}
}
