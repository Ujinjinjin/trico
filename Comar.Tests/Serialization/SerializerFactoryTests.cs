using Comar.Constants;
using Comar.Serialization;
using System;

namespace Comar.Tests.Serialization;

public class SerializerFactoryTests : UnitTestBase
{
	[Theory]
	[InlineData(FilenameExtension.Json)]
	[InlineData(FilenameExtension.Yaml)]
	[InlineData(FilenameExtension.Yml)]
	public void SerializerFactoryTests__CreateSerializer__WhenFilepathWithSupportedExtensionGiven_ThenSerializerCreated(string filepath)
	{
		// arrange
		var serializerFactory = new SerializerFactory();
		
		// act
		var serializer = serializerFactory.CreateSerializer(filepath);

		// assert
		serializer.Should().NotBeNull();
		serializer.Should().BeAssignableTo<ISerializer>();
	}

	[Fact]
	public void SerializerFactoryTests__CreateSerializer__WhenFilepathWithUnsupportedExtensionGiven_ThenExceptionThrown()
	{
		// arrange
		var serializerFactory = new SerializerFactory();
		var name = Fixture.Create<string>();
		var extension = Fixture.Create<string>();
		var filepath = $"{name}.{extension}";
		var action = () => serializerFactory.CreateSerializer(filepath);

		// act & assert
		action.Should().Throw<ArgumentOutOfRangeException>();
	}

	[Theory]
	[InlineData("")]
	[InlineData(null)]
	[InlineData(" ")]
	public void SerializerFactoryTests__CreateSerializer__WhenEmptyFilepathGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var serializerFactory = new SerializerFactory();
		var action = () => serializerFactory.CreateSerializer(filepath);

		// act & assert
		action.Should().Throw<ArgumentNullException>();
	}
}