using Comar.Containers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;

namespace Comar.Tests.Containers;

public class JsonObjectTests : UnitTestBase
{
	[Fact]
	public void JsonObjectTests_ctor__WhenNullObjectGiven_ThenEmptyJsonObjectCreated()
	{
		// arrange
		var jsonObject = new JsonObject(null);
		var expected = "{}";

		// act
		var ser = JsonSerializer.Serialize(jsonObject);

		// assert
		ser.Should().Be(expected);
	}

	[Fact]
	public void JsonObjectTests_TryGet__WhenEmptyJsonObjectGiven_ThenValueNotReturned()
	{
		// arrange
		var key = Fixture.Create<string>();
		var jsonObject = new JsonObject();

		// act
		var result = jsonObject.TryGet(key, out var value);

		// assert
		result.Should().BeFalse();
		value.Should().BeNull();
	}

	[Fact]
	public void JsonObjectTests_TryGet__WhenNotExistingKeyGiven_ThenValueNotReturned()
	{
		// arrange
		var keyValuePairs = Fixture.Create<KeyValuePair<string, string>[]>();
		var key = Fixture.Create<string>();
		var jsonObject = CreateJsonObject(keyValuePairs);

		// act
		var result = jsonObject.TryGet(key, out var value);

		// assert
		result.Should().BeFalse();
		value.Should().BeNull();
	}

	[Fact]
	public void JsonObjectTests_TryGet__WhenExistingKeyGiven_ThenValueReturned()
	{
		// arrange
		var keyValuePairs = Fixture.Create<KeyValuePair<string, string>[]>();
		var jsonObject = CreateJsonObject(keyValuePairs);

		// act
		foreach (var (key, expected) in keyValuePairs)
		{
			var result = jsonObject.TryGet(key, out var value);

			// assert
			result.Should().BeTrue();
			value.Should().Be(expected);
		}
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void JsonObjectTests_TryGet__WhenEmptyKeyGiven_ThenValueNotReturned(string key)
	{
		// arrange
		// arrange
		var keyValuePairs = Fixture.Create<KeyValuePair<string, string>[]>();
		var jsonObject = CreateJsonObject(keyValuePairs);

		// act
		var result = jsonObject.TryGet(key, out var value);

		// assert
		result.Should().BeFalse();
		value.Should().BeNull();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void JsonObjectTests_Set__WhenEmptyKeyGiven_ThenExceptionThrown(string key)
	{
		// arrange
		var jsonObject = new JsonObject();
		var value = Fixture.Create<string>();
		var action = () => jsonObject.Set(key, value);

		// act & assert
		action.Should().Throw<ArgumentNullException>();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void JsonObjectTests_Set__WhenEmptyValueGiven_ThenExceptionThrown(string value)
	{
		// arrange
		var key = Fixture.Create<string>();
		var jsonObject = new JsonObject();
		var action = () => jsonObject.Set(key, value);

		// act & assert
		action.Should().Throw<ArgumentNullException>();
	}

	[Fact]
	public void JsonObjectTests_Set__WhenGivenNestedKeyContainsPathToExistingPlainKey_ThenExceptionThrown()
	{
		// arrange
		var keyValuePairs = Fixture.Create<KeyValuePair<string, string>[]>();
		var nestedKey = $"{keyValuePairs[0].Key}.{Fixture.Create<string>()}";
		var value = Fixture.Create<string>();
		var jsonObject = CreateJsonObject(keyValuePairs);
		var action = () => jsonObject.Set(nestedKey, value);

		// act & assert
		action.Should().Throw<JsonException>();
	}

	[Fact]
	public void JsonObjectTests_Set__WhenNestedKeyGiven_ThenValueSet()
	{
		// arrange
		var key = string.Join('.', Fixture.Create<string>(), Fixture.Create<string>());
		var value = Fixture.Create<string>();
		var jsonObject = new JsonObject();
		var action = () => jsonObject.Set(key, value);

		// act & assert
		action.Should().NotThrow();
	}

	private JsonObject CreateJsonObject(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
	{
		var jsonObject = new JsonObject();
		foreach (var (key, value) in keyValuePairs)
		{
			jsonObject.Set(key, value);
		}

		return jsonObject;
	}
}
