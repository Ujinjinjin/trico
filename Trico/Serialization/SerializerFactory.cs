﻿using Trico.Constants;

namespace Trico.Serialization;

/// <inheritdoc />
public sealed class SerializerFactory : ISerializerFactory
{
	/// <inheritdoc />
	public ISerializer CreateSerializer(string filepath)
	{
		if (string.IsNullOrWhiteSpace(filepath))
		{
			throw new ArgumentNullException(nameof(filepath));
		}

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
