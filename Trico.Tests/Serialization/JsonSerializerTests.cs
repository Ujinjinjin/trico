using Trico.Serialization;
using System.Runtime.Serialization;

namespace Trico.Tests.Serialization;

public class JsonSerializerTests : UnitTestBase
{
	[Fact]
	public void JsonSerializerTests__Serialize__WhenNullGiven_ThenExceptionThrown()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var action = () => jsonSerializer.Serialize<TestStruct?>(null);

		// Act & Assert
		action.Should().Throw<SerializationException>();
	}

	[Fact]
	public void JsonSerializerTests__Serialize__WhenValidObjectGiven_ThenObjectSerialized()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var data = Fixture.Create<TestStruct>();
		var expectedSerializedData = $"{{\n  \"property1\": \"{data.Property1}\",\n  \"property2\": \"{data.Property2}\"\n}}";

		// Act
		var serializedData = jsonSerializer.Serialize(data);

		// Assert
		serializedData.Should().Be(expectedSerializedData);
	}

	[Fact]
	public void JsonSerializerTests__Serialize__WhenStructWithNullValuePropertiesGiven_ThenObjectSerializedWithoutNullValues()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var data = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = null,
		};
		var expectedSerializedData = $"{{\n  \"property1\": \"{data.Property1}\"\n}}";

		// Act
		var serializedData = jsonSerializer.Serialize(data);

		// Assert
		serializedData.Should().Be(expectedSerializedData);
	}

	[Fact]
	public void JsonSerializerTests__Deserialize__WhenSerializedDataWithMissingOfPropertiesGiven_ThenObjectDeserializedAndMissingPropertiesAreNull()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var expectedData = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = null
		};
		var serializedData = $"{{\n  \"Property1\": \"{expectedData.Property1}\"\n}}";

		// Act
		var data = jsonSerializer.Deserialize<TestStruct>(serializedData);

		// Assert
		data.Should().Be(expectedData);
	}

	[Fact]
	public void JsonSerializerTests__Deserialize__WhenSufficientSerializedDataGiven_ThenObjectDeserializedWithAllProperties()
	{
		// Arrange
		var jsonSerializer = new JsonSerializer();
		var expectedData = new TestStruct
		{
			Property1 = Fixture.Create<string>(),
			Property2 = Fixture.Create<string>(),
		};
		var serializedData = $"{{\n  \"Property1\": \"{expectedData.Property1}\",\n  \"Property2\": \"{expectedData.Property2}\"\n}}";

		// Act
		var data = jsonSerializer.Deserialize<TestStruct>(serializedData);

		// Assert
		data.Should().Be(expectedData);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void JsonSerializerTests_Deserialize__WhenEmptyOrNullContentGiven_ThenExceptionThrown(string content)
	{
		// arrange
		var jsonSerializer = new JsonSerializer();
		var action = () => jsonSerializer.Deserialize<TestStruct>(content);

		// act & assert
		action.Should().Throw<SerializationException>();
	}

	[Fact]
	public void JsonSerializerTests_Deserialize__WhenNonParsableContentGiven_ThenExceptionThrown()
	{
		// arrange
		var jsonSerializer = new JsonSerializer();
		var content = Fixture.Create<string>();
		var action = () => jsonSerializer.Deserialize<TestStruct>(content);

		// act & assert
		action.Should().Throw<SerializationException>();
	}
}
