using Comar.Adapters;
using Comar.Configuration;
using Comar.Configuration.Providers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Comar.Tests.Providers;

public class EnvironmentVariableConfigurationProviderTests : UnitTestBase
{
	private readonly string _prefix;

	public EnvironmentVariableConfigurationProviderTests()
	{
		_prefix = Fixture.Create<string>();
	}

	[Fact]
	public async Task EnvironmentVariableConfigurationProviderTests_LoadAsync__Smoke()
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateEmptyOptions();
		var token = Fixture.Create<CancellationToken>();
		var action = async () => await configurationProvider.LoadAsync(
			options,
			token
		);

		// act & assert
		await action.Should().NotThrowAsync();
	}

	[Fact]
	public void EnvironmentVariableConfigurationProviderTests_Load__Smoke()
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateEmptyOptions();
		var action = () => configurationProvider.Load(
			options
		);

		// act & assert
		action.Should().NotThrow();
	}

	[Fact]
	public async Task EnvironmentVariableConfigurationProviderTests_LoadAsync__WhenPrefixNotProvided_ThenAllVariablesImported()
	{
		// arrange
		var variables = CreateDefaultVariables();
		var configurationProvider = CreateConfigurationProvider(variables);
		var options = CreateEmptyOptions();
		var token = Fixture.Create<CancellationToken>();

		// act
		await configurationProvider.LoadAsync(
			options,
			token
		);

		// assert
		foreach (var (key, expected) in variables)
		{
			var result = configurationProvider.TryGet(key, out var value);

			result.Should().BeTrue();
			value.Should().Be(expected);
		}
	}

	[Fact]
	public void EnvironmentVariableConfigurationProviderTests_Load__WhenPrefixNotProvided_ThenAllVariablesImported()
	{
		// arrange
		var variables = CreateDefaultVariables();
		var configurationProvider = CreateConfigurationProvider(variables);
		var options = CreateEmptyOptions();

		// act
		configurationProvider.Load(
			options
		);

		// assert
		foreach (var (key, expected) in variables)
		{
			var result = configurationProvider.TryGet(key, out var value);

			result.Should().BeTrue();
			value.Should().Be(expected);
		}
	}

	[Theory]
	[InlineData("property-1", "value-1")]
	[InlineData("property-2", "value-2")]
	[InlineData("property-3", "value-3")]
	[InlineData("property-5", "value-5")]
	public async Task EnvironmentVariableConfigurationProviderTests__LoadAsync__WhenPrefixGiven_ThenVariablesWithPrefixLoadedAndPrefixOmitted(
		string key,
		string expectedValue
	)
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();
		var token = Fixture.Create<CancellationToken>();

		// act
		await configurationProvider.LoadAsync(
			options,
			token
		);
		var result = configurationProvider.TryGet(key, out var value);

		// assert
		result.Should().BeTrue();
		value.Should().Be(expectedValue);
	}

	[Theory]
	[InlineData("property-1", "value-1")]
	[InlineData("property-2", "value-2")]
	[InlineData("property-3", "value-3")]
	[InlineData("property-5", "value-5")]
	public void EnvironmentVariableConfigurationProviderTests__Load__WhenPrefixGiven_ThenVariablesWithPrefixLoadedAndPrefixOmitted(
		string key,
		string expectedValue
	)
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();

		// act
		configurationProvider.Load(
			options
		);
		var result = configurationProvider.TryGet(key, out var value);

		// assert
		result.Should().BeTrue();
		value.Should().Be(expectedValue);
	}

	[Theory]
	[InlineData("property-6")]
	public async Task EnvironmentVariableConfigurationProviderTests__LoadAsync__WhenPrefixGiven_ThenVariablesWithoutPrefixNotLoaded(
		string key
	)
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();
		var token = Fixture.Create<CancellationToken>();

		// act
		await configurationProvider.LoadAsync(
			options,
			token
		);
		var result = configurationProvider.TryGet(key, out var value);

		// assert
		result.Should().BeFalse();
		value.Should().BeNull();
	}

	[Theory]
	[InlineData("property-6")]
	public void EnvironmentVariableConfigurationProviderTests__Load__WhenPrefixGiven_ThenVariablesWithoutPrefixNotLoaded(
		string key
	)
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();

		// act
		configurationProvider.Load(
			options
		);
		var result = configurationProvider.TryGet(key, out var value);

		// assert
		result.Should().BeFalse();
		value.Should().BeNull();
	}

	[Fact]
	public async Task EnvironmentVariableConfigurationProviderTests__DumpAsync__WhenDumpedAfterChangingConfigs_ThenChangedConfigsLoadedNextTime()
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();
		var token = Fixture.Create<CancellationToken>();
		var newKey = Fixture.Create<string>();
		var newValue = Fixture.Create<string>();

		await configurationProvider.LoadAsync(options, token);
		configurationProvider.Set(newKey, newValue);

		// act
		await configurationProvider.DumpAsync(token);
		await configurationProvider.LoadAsync(options, token);

		var result = configurationProvider.TryGet(newKey, out var value);

		// assert
		result.Should().BeTrue();
		value.Should().Be(newValue);
	}

	[Fact]
	public void EnvironmentVariableConfigurationProviderTests__Dump__WhenDumpedAfterChangingConfigs_ThenChangedConfigsLoadedNextTime()
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();
		var newKey = Fixture.Create<string>();
		var newValue = Fixture.Create<string>();

		configurationProvider.Load(options);
		configurationProvider.Set(newKey, newValue);

		// act
		configurationProvider.Dump();
		configurationProvider.Load(options);

		var result = configurationProvider.TryGet(newKey, out var value);

		// assert
		result.Should().BeTrue();
		value.Should().Be(newValue);
	}

	[Theory]
	[InlineData("property-1", "value-1")]
	[InlineData("property-2", "value-2")]
	[InlineData("property-3", "value-3")]
	[InlineData("property-5", "value-5")]
	public async Task EnvironmentVariableConfigurationProviderTests__TryGet__WhenExistingKeyGiven_ThenConfigReturned(
		string key,
		string expectedValue
	)
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();
		var token = Fixture.Create<CancellationToken>();

		await configurationProvider.LoadAsync(options, token);

		// act
		var result = configurationProvider.TryGet(key, out var value);

		// assert
		result.Should().BeTrue();
		value.Should().Be(expectedValue);
	}

	[Fact]
	public void EnvironmentVariableConfigurationProviderTests__TryGet__WhenNotExistingKeyGiven_ThenConfigNotReturned()
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
	[InlineData("property-1")]
	[InlineData("property-2")]
	[InlineData("property-3")]
	[InlineData("property-5")]
	public async Task EnvironmentVariableConfigurationProviderTests__Set__WhenExistingKeyGiven_ThenValueUpdated(
		string key
	)
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();
		var token = Fixture.Create<CancellationToken>();
		var value = Fixture.Create<string>();

		await configurationProvider.LoadAsync(options, token);

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
	public async Task EnvironmentVariableConfigurationProviderTests__Set__WhenNotExistingKeyGiven_ThenValueCreated()
	{
		// arrange
		var configurationProvider = CreateConfigurationProvider();
		var options = CreateValidOptions();
		var token = Fixture.Create<CancellationToken>();
		var key = Fixture.Create<string>();
		var value = Fixture.Create<string>();

		await configurationProvider.LoadAsync(options, token);

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

	private IDictionary<string, string> CreateValidOptions()
	{
		var options = new Dictionary<string, string> { { "prefix", _prefix } };
		return options;
	}

	private IDictionary<string, string> CreateEmptyOptions()
	{
		var options = new Dictionary<string, string>();
		return options;
	}

	private IConfigurationProvider CreateConfigurationProvider(IDictionary<string, string>? variables = null)
	{
		var env = CreateConfiguredEnvironmentMock(variables);
		return new EnvironmentVariableConfigurationProvider(env);
	}

	private IEnvironment CreateConfiguredEnvironmentMock(IDictionary<string, string>? variables = null)
	{
		variables ??= CreateDefaultVariables();
		var environment = new Mock<IEnvironment>();

		environment
			.Setup(x => x.GetEnvironmentVariables())
			.Returns(() => variables);
		environment
			.Setup(x => x.SetEnvironmentVariable(It.IsAny<string>(), It.IsAny<string>()))
			.Callback(new Action<string, string>((variable, value) => variables.TryAdd(variable, value)));

		return environment.Object;
	}

	private IDictionary<string, string> CreateDefaultVariables()
	{
		var variables = new ConcurrentDictionary<string, string>();

		variables.TryAdd($"{_prefix}property-1", "value-1");
		variables.TryAdd($"{_prefix}property-2", "value-2");
		variables.TryAdd($"{_prefix}property-3", "value-3");
		variables.TryAdd($"{Fixture.Create<string>()}property-4", "value-4");
		variables.TryAdd($"{_prefix}property-5", "value-5");
		variables.TryAdd("property-6", "value-6");

		return variables;
	}
}
