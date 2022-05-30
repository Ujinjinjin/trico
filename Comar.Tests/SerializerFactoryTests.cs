using Comar.Constants;
using Comar.Factories;
using Comar.Serializers;
using Comar.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Comar.Tests;

public class SerializerFactoryTests
{
	private readonly ISerializerFactory _serializerFactory;

	public SerializerFactoryTests()
	{
		var serviceProvider = new IocModule().Build();

		_serializerFactory = serviceProvider.GetService<ISerializerFactory>() ?? throw new ArgumentNullException($"{nameof(ISerializerFactory)} is not registered");
	}

	[Theory]
	[InlineData(FilenameExtension.Json)]
	[InlineData(FilenameExtension.Yaml)]
	[InlineData(FilenameExtension.Yml)]
	public void SerializerFactoryTests__CreateSerializer__WhenFilepathWithSupportedExtensionGiven_ThenSerializerCreated(string filepath)
	{
		// act
		var serializer = _serializerFactory.CreateSerializer(filepath);

		// assert
		Assert.NotNull(serializer);
		Assert.IsAssignableFrom<ISerializer>(serializer);
	}

	[Fact]
	public void SerializerFactoryTests__CreateSerializer__WhenFilepathWithUnsupportedExtensionGiven_ThenExceptionThrown()
	{
		// arrange
		var filepath = $"{Guid.NewGuid():N}.{Guid.NewGuid():N}";

		// act & assert
		Assert.Throws<ArgumentOutOfRangeException>(() => _serializerFactory.CreateSerializer(filepath));
	}

	[Theory]
	[InlineData("")]
	[InlineData(null)]
	[InlineData(" ")]
	public void SerializerFactoryTests__CreateSerializer__WhenEmptyFilepathGiven_ThenExceptionThrown(string filepath)
	{
		// act & assert
		Assert.Throws<ArgumentNullException>(() => _serializerFactory.CreateSerializer(filepath));
	}
}