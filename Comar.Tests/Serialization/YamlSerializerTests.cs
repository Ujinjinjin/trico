using Comar.Serialization;
using System.Runtime.Serialization;

namespace Comar.Tests.Serialization;

public class YamlSerializerTests : UnitTestBase
{
	[Fact]
	public void JsonSerializerTests__Serialize__WhenNullGiven_ThenExceptionThrown()
	{
		// Arrange
		var jsonSerializer = new YamlSerializer();
		var action = () => jsonSerializer.Serialize<TestStruct?>(null);

		// Act & Assert
		action.Should().Throw<SerializationException>();
	}

	[Fact]
	public void YamlSerializerTests__Serialize__WhenValidObjectGiven_ThenObjectSerialized()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var data = Fixture.Create<TestStruct>();
		var expectedSerializedData = $"property1: {data.Property1}\nproperty2: {data.Property2}\n";

		// Act
		var serializedData = yamlSerializer.Serialize(data);

		// Assert
		serializedData.Should().Be(expectedSerializedData);
	}

	[Fact]
	public void YamlSerializerTests__Serialize__WhenStructWithNullValuePropertiesGiven_ThenObjectSerializedWithoutNullValues()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var data = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = null,
		};
		var expectedSerializedData = $"property1: {data.Property1}\n";

		// Act
		var serializedData = yamlSerializer.Serialize(data);

		// Assert
		serializedData.Should().Be(expectedSerializedData);
	}

	[Fact]
	public void YamlSerializerTests__Deserialize__WhenSerializedDataWithMissingOfPropertiesGiven_ThenObjectDeserializedAndMissingPropertiesAreNull()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var expectedData = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = null
		};
		var serializedData = $"property1: {expectedData.Property1}\n";

		// Act
		var data = yamlSerializer.Deserialize<TestStruct>(serializedData);

		// Assert
		data.Should().Be(expectedData);
	}

	[Fact]
	public void YamlSerializerTests__Deserialize__WhenSufficientSerializedDataGiven_ThenObjectDeserializedWithAllProperties()
	{
		// Arrange
		var yamlSerializer = new YamlSerializer();
		var expectedData = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = Fixture.Create<string>(),
		};
		var serializedData = $"property1: {expectedData.Property1}\nproperty2: {expectedData.Property2}\n";

		// Act
		var data = yamlSerializer.Deserialize<TestStruct>(serializedData);

		// Assert
		data.Should().Be(expectedData);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void YamlSerializerTests_Deserialize__WhenEmptyOrNullContentGiven_ThenExceptionThrown(string content)
	{
		// arrange
		var yamlSerializer = new YamlSerializer();
		var action = () => yamlSerializer.Deserialize<TestStruct>(content);

		// act & assert
		action.Should().Throw<SerializationException>();
	}

	[Fact]
	public void YamlSerializerTests_Deserialize__WhenNonParsableContentGiven_ThenExceptionThrown()
	{
		// arrange
		var yamlSerializer = new YamlSerializer();
		var content = Fixture.Create<string>();
		var action = () => yamlSerializer.Deserialize<TestStruct>(content);

		// act & assert
		action.Should().Throw<SerializationException>();
	}
}
