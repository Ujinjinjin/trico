using Comar.Configuration;
using Comar.Configuration.Providers;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Comar.Tests.Providers;

public class InMemoryConfigurationProviderTests : UnitTestBase
{
	[Fact]
	public void InMemoryConfigurationProviderTests__TryGet__WhenNotExistingKeyGiven_ThenConfigNotReturned()
	{
		// arrange
		var key = Fixture.Create<string>();
		var configurationProvider = CreateConfigurationProvider();

		// act
		var result = configurationProvider.TryGet(key, out var value);

		// assert
		result.Should().BeFalse();
		value.Should().BeNull();
	}

	[Theory]
	[InlineData("property-1", "value-1")]
	[InlineData("property-2", "value-2")]
	[InlineData("property-3", "value-3")]
	[InlineData("property-4", "value-4")]
	[InlineData("property-5", "value-5")]
	public void InMemoryConfigurationProviderTests__TryGet__WhenExistingKeyGiven_ThenConfigReturned(
		string key,
		string expectedValue
	)
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();

		// act
		var result = configurationProvider.TryGet(key, out var actualValue);

		// assert
		result.Should().BeTrue();
		actualValue.Should().NotBeNull();
		actualValue.Should().Be(expectedValue);
	}

	[Theory]
	[InlineData("property-1")]
	[InlineData("property-2")]
	[InlineData("property-3")]
	[InlineData("property-4")]
	[InlineData("property-5")]
	public void InMemoryConfigurationProviderTests__Set__WhenExistingKeyGiven_ThenValueUpdated(
		string key
	)
	{
		// arrange
		var value = Fixture.Create<string>();
		var configurationProvider = CreateConfigurationProvider();

		// act
		var oldResult = configurationProvider.TryGet(key, out var oldValue);
		configurationProvider.Set(key, value);
		var newResult = configurationProvider.TryGet(key, out var newValue);

		// assert
		oldResult.Should().BeTrue();
		newResult.Should().BeTrue();
		oldValue.Should().NotBeNull();
		newValue.Should().NotBeNull();

		newValue.Should().NotBe(oldValue);
		newValue.Should().Be(value);
	}

	[Fact]
	public void InMemoryConfigurationProviderTests__Set__WhenNotExistingKeyGiven_ThenValueCreated()
	{
		// arrange
		var key = Fixture.Create<string>();
		var value = Fixture.Create<string>();
		var configurationProvider = CreateConfigurationProvider();

		// act
		var oldResult = configurationProvider.TryGet(key, out var oldValue);
		configurationProvider.Set(key, value);
		var newResult = configurationProvider.TryGet(key, out var newValue);

		// assert
		oldResult.Should().BeFalse();
		newResult.Should().BeTrue();
		oldValue.Should().BeNull();
		newValue.Should().NotBeNull();

		newValue.Should().NotBe(oldValue);
		newValue.Should().Be(value);
	}

	[Fact]
	public void InMemoryConfigurationProviderTests__LoadAsync__WhenConfigurationLoadedWithDifferentOptions_ThenNewConfigKeyExists()
	{
		// arrange
		var key = Fixture.Create<string>();
		var value = Fixture.Create<string>();
		var options = new Dictionary<string, string> { { key, value } };
		var configurationProvider = CreateConfigurationProvider(options);

		// act
		var result = configurationProvider.TryGet(key, out var optionValue);

		// assert
		result.Should().BeTrue();
		optionValue.Should().NotBeNull();

		optionValue.Should().Be(value);
	}

	[Theory]
	[InlineData("property-1")]
	[InlineData("property-2")]
	[InlineData("property-3")]
	[InlineData("property-4")]
	[InlineData("property-5")]
	public void InMemoryConfigurationProviderTests__LoadAsync__WhenConfigurationLoadedWithDifferentOptions_ThenOldConfigKeyNotExists(string lookupKey)
	{
		// arrange
		var key = Fixture.Create<string>();
		var value = Fixture.Create<string>();
		var options = new Dictionary<string, string> { { key, value } };
		var configurationProvider = CreateConfigurationProvider(options);

		// act
		var result = configurationProvider.TryGet(lookupKey, out var optionValue);

		// assert
		result.Should().BeFalse();
		optionValue.Should().BeNull();
	}

	private IConfigurationProvider CreateConfigurationProvider(IDictionary<string, string>? options = null)
	{
		options ??= CreateDefaultOptions();
		return new InMemoryConfigurationProvider(options);
	}

	private IDictionary<string, string> CreateDefaultOptions()
	{
		var configuration = new ConcurrentDictionary<string, string>();

		configuration.TryAdd("property-1", "value-1");
		configuration.TryAdd("property-2", "value-2");
		configuration.TryAdd("property-3", "value-3");
		configuration.TryAdd("property-4", "value-4");
		configuration.TryAdd("property-5", "value-5");

		return configuration;
	}
}