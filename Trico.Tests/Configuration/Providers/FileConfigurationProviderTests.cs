using Trico.Configuration;
using Trico.Configuration.Providers;
using Trico.Constants;
using Trico.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Trico.Tests.Configuration.Providers;

public class FileConfigurationProviderTests : UnitTestBase
{
	private readonly IConfigurationProvider _configurationProvider;

	public FileConfigurationProviderTests()
	{
		var serviceProvider = new IocModule().Build();

		var configProviders = serviceProvider.GetServices<IConfigurationProvider>();
		var configProvider = configProviders.FirstOrDefault(x => x is FileConfigurationProvider);

		_configurationProvider = configProvider ?? throw new ArgumentNullException($"{nameof(FileConfigurationProvider)} is not registered");
	}

	[Fact]
	public async Task FileConfigurationProviderTests__LoadAsync__WhenFilepathKeyNotGiven_ThenExceptionThrown()
	{
		// arrange
		var options = CreateEmptyOptions();
		var action = async () => await _configurationProvider.LoadAsync(options, default);

		// act & assert
		await action.Should().ThrowAsync<KeyNotFoundException>();
	}

	[Fact]
	public void FileConfigurationProviderTests__Load__WhenFilepathKeyNotGiven_ThenExceptionThrown()
	{
		// arrange
		var options = CreateEmptyOptions();
		var action = () => _configurationProvider.Load(options);

		// act & assert
		action.Should().Throw<KeyNotFoundException>();
	}

	[Theory]
	[InlineData(FilenameExtension.Json)]
	[InlineData(FilenameExtension.Yaml)]
	[InlineData(FilenameExtension.Yml)]
	public async Task FileConfigurationProviderTests_LoadAsync__WhenNotExistingFilepathGiven_ThenProviderLoaded(string extension)
	{
		// arrange
		var filename = Fixture.Create<string>();
		var filepath = $"./InputData/{filename}.{extension}";
		var options = CreateValidOptions(filepath);
		var action = async () => await _configurationProvider.LoadAsync(options, default);

		// act & assert
		await action.Should().NotThrowAsync();
	}

	[Theory]
	[InlineData("./InputData/invalid-config.json")]
	[InlineData("./InputData/invalid-config.yml")]
	[InlineData("./InputData/invalid-config.yaml")]
	public async Task FileConfigurationProviderTests__LoadAsync__WhenFilepathWithInvalidContentGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		var action = async () => await _configurationProvider.LoadAsync(options, default);

		// act & assert
		await action.Should().ThrowAsync<FileLoadException>();
	}

	[Theory]
	[InlineData("./InputData/invalid-config.json")]
	[InlineData("./InputData/invalid-config.yml")]
	[InlineData("./InputData/invalid-config.yaml")]
	public void FileConfigurationProviderTests__Load__WhenFilepathWithInvalidContentGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		var action = () => _configurationProvider.Load(options);

		// act & assert
		action.Should().Throw<FileLoadException>();
	}

	[Theory]
	[InlineData("./InputData/empty-config.json")]
	[InlineData("./InputData/empty-config.yml")]
	[InlineData("./InputData/empty-config.yaml")]
	public async Task FileConfigurationProviderTests__LoadAsync__WhenFilepathWithEmptyContentGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		var action = async () => await _configurationProvider.LoadAsync(options, default);

		// act & assert
		await action.Should().ThrowAsync<FileLoadException>();
	}

	[Theory]
	[InlineData("./InputData/empty-config.json")]
	[InlineData("./InputData/empty-config.yml")]
	[InlineData("./InputData/empty-config.yaml")]
	public void FileConfigurationProviderTests__Load__WhenFilepathWithEmptyContentGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		var action = () => _configurationProvider.Load(options);

		// act & asser
		action.Should().Throw<FileLoadException>();
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public async Task FileConfigurationProviderTests__LoadAsync__WhenFilepathWithCorrectContentGiven_ThenConfigLoaded(string filepath)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		var action = async () => await _configurationProvider.LoadAsync(options, default);

		// act & assert
		await action.Should().NotThrowAsync();
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public void FileConfigurationProviderTests__Load__WhenFilepathWithCorrectContentGiven_ThenConfigLoaded(string filepath)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		var action = () => _configurationProvider.Load(options);

		// act & assert
		action.Should().NotThrow();
	}

	[Fact]
	public async Task FileConfigurationProviderTests__DumpAsync__WhenDumpedBeforeLoading_ThenExceptionThrown()
	{
		// arrange
		var action = async () => await _configurationProvider.DumpAsync(default);
		// act & assert
		await action.Should().ThrowAsync<FileLoadException>();
	}

	[Fact]
	public void FileConfigurationProviderTests__Dump__WhenDumpedBeforeLoading_ThenExceptionThrown()
	{
		// arrange
		var action = () => _configurationProvider.Dump();
		// act & assert
		action.Should().Throw<FileLoadException>();
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public async Task FileConfigurationProviderTests__DumpAsync__WhenDumpedAfterChangingConfigs_ThenChangedConfigsLoadedNextTime(string filepath)
	{
		// arrange
		var newKey = Fixture.Create<string>();
		var newValue = Fixture.Create<string>();
		var options = CreateValidOptions(filepath);

		await _configurationProvider.LoadAsync(options, default);
		_configurationProvider.Set(newKey, newValue);

		// act
		await _configurationProvider.DumpAsync(default);
		_configurationProvider.Load(options);

		var result = _configurationProvider.TryGet(newKey, out var value);

		// assert
		result.Should().BeTrue();
		value.Should().Be(newValue);
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public void FileConfigurationProviderTests__Dump__WhenDumpedAfterChangingConfigs_ThenChangedConfigsLoadedNextTime(string filepath)
	{
		// arrange
		var newKey = Fixture.Create<string>();
		var newValue = Fixture.Create<string>();
		var options = CreateValidOptions(filepath);

		_configurationProvider.Load(options);
		_configurationProvider.Set(newKey, newValue);

		// act
		_configurationProvider.Dump();
		_configurationProvider.Load(options);

		var result = _configurationProvider.TryGet(newKey, out var value);

		// assert
		result.Should().BeTrue();
		value.Should().Be(newValue);
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public async Task FileConfigurationProviderTests__TryGet__WhenNotExistingKeyGiven_ThenConfigNotReturned(string filepath)
	{
		// arrange
		var key = Fixture.Create<string>();
		var options = CreateValidOptions(filepath);
		await _configurationProvider.LoadAsync(options, default);

		// act
		var result = _configurationProvider.TryGet(key, out var value);

		// assert
		result.Should().BeFalse();
		value.Should().BeNull();
	}

	[Theory]
	[InlineData("./InputData/valid-config.json", "node.property", "value")]
	[InlineData("./InputData/valid-config.json", "node.node.property", "value")]
	[InlineData("./InputData/valid-config.json", "property", "value")]
	[InlineData("./InputData/valid-config.yml", "node.property", "value")]
	[InlineData("./InputData/valid-config.yml", "node.node.property", "value")]
	[InlineData("./InputData/valid-config.yml", "property", "value")]
	[InlineData("./InputData/valid-config.yaml", "node.property", "value")]
	[InlineData("./InputData/valid-config.yaml", "node.node.property", "value")]
	[InlineData("./InputData/valid-config.yaml", "property", "value")]
	public async Task FileConfigurationProviderTests__TryGet__WhenExistingKeyGiven_ThenConfigReturned(
		string filepath,
		string key,
		string expectedValue
	)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		await _configurationProvider.LoadAsync(options, default);

		// act
		var result = _configurationProvider.TryGet(key, out var actualValue);

		// assert
		result.Should().BeTrue();
		actualValue.Should().NotBeNull();
		actualValue.Should().Be(expectedValue);
	}

	[Theory]
	[InlineData("./InputData/valid-config.json", "node.property")]
	[InlineData("./InputData/valid-config.json", "node.node.property")]
	[InlineData("./InputData/valid-config.json", "property")]
	[InlineData("./InputData/valid-config.yml", "node.property")]
	[InlineData("./InputData/valid-config.yml", "node.node.property")]
	[InlineData("./InputData/valid-config.yml", "property")]
	[InlineData("./InputData/valid-config.yaml", "node.property")]
	[InlineData("./InputData/valid-config.yaml", "node.node.property")]
	[InlineData("./InputData/valid-config.yaml", "property")]
	public async Task FileConfigurationProviderTests__Set__WhenExistingKeyGiven_ThenValueUpdated(
		string filepath,
		string key
	)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		var value = Fixture.Create<string>();
		await _configurationProvider.LoadAsync(options, default);

		// act
		var oldResult = _configurationProvider.TryGet(key, out var oldValue);
		_configurationProvider.Set(key, value);
		var newResult = _configurationProvider.TryGet(key, out var newValue);

		// assert
		oldResult.Should().BeTrue();
		newResult.Should().BeTrue();
		oldValue.Should().NotBeNull();
		newValue.Should().NotBeNull();

		newValue.Should().NotBe(oldValue);
		newValue.Should().Be(value);
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public async Task FileConfigurationProviderTests__Set__WhenNotExistingKeyGiven_ThenValueCreated(
		string filepath
	)
	{
		// arrange
		var options = CreateValidOptions(filepath);
		var key = Fixture.Create<string>();
		var value = Fixture.Create<string>();
		await _configurationProvider.LoadAsync(options, default);

		// act
		var oldResult = _configurationProvider.TryGet(key, out var oldValue);
		_configurationProvider.Set(key, value);
		var newResult = _configurationProvider.TryGet(key, out var newValue);

		// assert
		oldResult.Should().BeFalse();
		newResult.Should().BeTrue();
		oldValue.Should().BeNull();
		newValue.Should().NotBeNull();

		newValue.Should().NotBe(oldValue);
		newValue.Should().Be(value);
	}

	private IDictionary<string, string> CreateEmptyOptions()
	{
		var options = new Dictionary<string, string>();
		return options;
	}

	private IDictionary<string, string> CreateValidOptions(string filepath)
	{
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};
		return options;
	}
}
