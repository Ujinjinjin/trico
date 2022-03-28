using Comar.Configuration;
using Comar.Configuration.Providers;
using Comar.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Comar.Tests.Providers;

public class InMemoryConfigurationProviderTests
{
	private readonly IConfigurationProvider _configurationProvider;

	public InMemoryConfigurationProviderTests()
	{
		var serviceProvider = new IocModule().Build();

		var configProviders = serviceProvider.GetServices<IConfigurationProvider>();
		var configProvider = configProviders.FirstOrDefault(x => x is InMemoryConfigurationProvider);

		_configurationProvider = configProvider ?? throw new ArgumentNullException($"{nameof(InMemoryConfigurationProvider)} is not registered");
	}

	[Fact]
	public void InMemoryConfigurationProviderTests__TryGet__WhenNotExistingKeyGiven_ThenConfigNotReturned()
	{
		// arrange
		var key = Guid.NewGuid().ToString("N");
		var options = new Dictionary<string, string>();
		// await _configurationProvider.LoadAsync(options, default);

		// act
		var result = _configurationProvider.TryGet(key, out var value);
		
		// assert
		Assert.False(result);
		Assert.Null(value);
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
		var options = new Dictionary<string, string>();
		// await _configurationProvider.LoadAsync(options, default);

		// act
		var result = _configurationProvider.TryGet(key, out var actualValue);
		
		// assert
		Assert.True(result);
		Assert.NotNull(actualValue);
		Assert.Equal(expectedValue, actualValue);
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
		var options = new Dictionary<string, string>();
		var value = Guid.NewGuid().ToString("N");
		// await _configurationProvider.LoadAsync(options, default);

		// act
		var oldResult = _configurationProvider.TryGet(key, out var oldValue);
		_configurationProvider.Set(key, value);
		var newResult = _configurationProvider.TryGet(key, out var newValue);
		
		// assert
		Assert.True(oldResult);
		Assert.True(newResult);
		Assert.NotNull(oldValue);
		Assert.NotNull(newValue);
		
		Assert.NotEqual(oldValue, newValue);
		Assert.Equal(value, newValue);
	}

	[Fact]
	public void InMemoryConfigurationProviderTests__Set__WhenNotExistingKeyGiven_ThenValueCreated()
	{
		// arrange
		var options = new Dictionary<string, string>();
		var key = Guid.NewGuid().ToString("N");
		var value = Guid.NewGuid().ToString("N");
		// await _configurationProvider.LoadAsync(options, default);

		// act
		var oldResult = _configurationProvider.TryGet(key, out var oldValue);
		_configurationProvider.Set(key, value);
		var newResult = _configurationProvider.TryGet(key, out var newValue);
		
		// assert
		Assert.False(oldResult);
		Assert.True(newResult);
		Assert.Null(oldValue);
		Assert.NotNull(newValue);
		
		Assert.NotEqual(oldValue, newValue);
		Assert.Equal(value, newValue);
	}

	[Fact]
	public async Task InMemoryConfigurationProviderTests__LoadAsync__WhenConfigurationLoadedWithDifferentOptions_ThenNewConfigKeyExists()
	{
		// arrange
		var key = Guid.NewGuid().ToString("N");
		var value = Guid.NewGuid().ToString("N");
		var options = new Dictionary<string, string>
		{
			{ key, value }
		};
		await _configurationProvider.LoadAsync(options, default);

		// act
		var result = _configurationProvider.TryGet(key, out var optionValue);
		
		// assert
		Assert.True(result);
		Assert.NotNull(optionValue);

		Assert.Equal(value, optionValue);
	}

	[Theory]
	[InlineData("property-1")]
	[InlineData("property-2")]
	[InlineData("property-3")]
	[InlineData("property-4")]
	[InlineData("property-5")]
	public async Task InMemoryConfigurationProviderTests__LoadAsync__WhenConfigurationLoadedWithDifferentOptions_ThenOldConfigKeyNotExists(string key)
	{
		// arrange
		var options = new Dictionary<string, string>
		{
			{ Guid.NewGuid().ToString("N"), Guid.NewGuid().ToString("N") }
		};
		await _configurationProvider.LoadAsync(options, default);

		// act
		var result = _configurationProvider.TryGet(key, out var optionValue);
		
		// assert
		Assert.False(result);
		Assert.Null(optionValue);
	}
}