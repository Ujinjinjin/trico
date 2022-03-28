using Comar.Configuration;
using Comar.Configuration.Providers;
using Comar.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Comar.Tests.Providers;

public class FileConfigurationProviderTests
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
		var options = new Dictionary<string, string>();

		// act & assert
		await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _configurationProvider.LoadAsync(options, default));
	}

	[Fact]
	public void FileConfigurationProviderTests__Load__WhenFilepathKeyNotGiven_ThenExceptionThrown()
	{
		// arrange
		var options = new Dictionary<string, string>();

		// act & assert
		Assert.Throws<KeyNotFoundException>(() => _configurationProvider.Load(options));
	}

	[Theory]
	[InlineData("./InputData/invalid-config.json")]
	[InlineData("./InputData/invalid-config.yml")]
	[InlineData("./InputData/invalid-config.yaml")]
	public async Task FileConfigurationProviderTests__LoadAsync__WhenFilepathWithInvalidContentGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};

		// act & assert
		await Assert.ThrowsAsync<FileLoadException>(async () => await _configurationProvider.LoadAsync(options, default));
	}

	[Theory]
	[InlineData("./InputData/invalid-config.json")]
	[InlineData("./InputData/invalid-config.yml")]
	[InlineData("./InputData/invalid-config.yaml")]
	public void FileConfigurationProviderTests__Load__WhenFilepathWithInvalidContentGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};

		// act & assert
		Assert.Throws<FileLoadException>(() => _configurationProvider.Load(options));
	}

	[Theory]
	[InlineData("./InputData/empty-config.json")]
	[InlineData("./InputData/empty-config.yml")]
	[InlineData("./InputData/empty-config.yaml")]
	public async Task FileConfigurationProviderTests__LoadAsync__WhenFilepathWithEmptyContentGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};

		// act & assert
		await Assert.ThrowsAsync<FileLoadException>(async () => await _configurationProvider.LoadAsync(options, default));
	}

	[Theory]
	[InlineData("./InputData/empty-config.json")]
	[InlineData("./InputData/empty-config.yml")]
	[InlineData("./InputData/empty-config.yaml")]
	public void FileConfigurationProviderTests__Load__WhenFilepathWithEmptyContentGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};

		// act & asser
		Assert.Throws<FileLoadException>(() => _configurationProvider.Load(options));
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public async Task FileConfigurationProviderTests__LoadAsync__WhenFilepathWithCorrectContentGiven_ThenConfigLoaded(string filepath)
	{
		// arrange
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};

		// act & assert
		await _configurationProvider.LoadAsync(options, default);
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public void FileConfigurationProviderTests__Load__WhenFilepathWithCorrectContentGiven_ThenConfigLoaded(string filepath)
	{
		// arrange
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};

		// act & assert
		_configurationProvider.Load(options);
	}

	[Fact]
	public async Task FileConfigurationProviderTests__DumpAsync__WhenDumpedBeforeLoading_ThenExceptionThrown()
	{
		// act & assert
		await Assert.ThrowsAsync<FileLoadException>(async () => await _configurationProvider.DumpAsync(default));
	}

	[Fact]
	public void FileConfigurationProviderTests__Dump__WhenDumpedBeforeLoading_ThenExceptionThrown()
	{
		// act & assert
		Assert.Throws<FileLoadException>(() => _configurationProvider.Dump());
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public async Task FileConfigurationProviderTests__DumpAsync__WhenDumpedAfterChangingConfigs_ThenChangedConfigsLoadedNextTime(string filepath)
	{
		// arrange
		var newValue = Guid.NewGuid().ToString("N");
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};

		await _configurationProvider.LoadAsync(options, default);
		_configurationProvider.Set("new-property", newValue);

		// act
		await _configurationProvider.DumpAsync(default);
		_configurationProvider.Load(options);
		
		// assert
		Assert.True(_configurationProvider.TryGet("new-property", out var value));
		Assert.Equal(newValue, value);
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public void FileConfigurationProviderTests__Dump__WhenDumpedAfterChangingConfigs_ThenChangedConfigsLoadedNextTime(string filepath)
	{
		// arrange
		var newValue = Guid.NewGuid().ToString("N");
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};

		_configurationProvider.Load(options);
		_configurationProvider.Set("new-property", newValue);

		// act
		_configurationProvider.Dump();
		_configurationProvider.Load(options);
		
		// assert
		Assert.True(_configurationProvider.TryGet("new-property", out var value));
		Assert.Equal(newValue, value);
	}

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public async Task FileConfigurationProviderTests__TryGet__WhenNotExistingKeyGiven_ThenConfigNotReturned(string filepath)
	{
		// arrange
		var key = Guid.NewGuid().ToString("N");
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};
		await _configurationProvider.LoadAsync(options, default);

		// act
		var result = _configurationProvider.TryGet(key, out var value);
		
		// assert
		Assert.False(result);
		Assert.Null(value);
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
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};
		await _configurationProvider.LoadAsync(options, default);

		// act
		var result = _configurationProvider.TryGet(key, out var actualValue);
		
		// assert
		Assert.True(result);
		Assert.NotNull(actualValue);
		Assert.Equal(expectedValue, actualValue);
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
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};
		var value = Guid.NewGuid().ToString("N");
		await _configurationProvider.LoadAsync(options, default);

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

	[Theory]
	[InlineData("./InputData/valid-config.json")]
	[InlineData("./InputData/valid-config.yml")]
	[InlineData("./InputData/valid-config.yaml")]
	public async Task FileConfigurationProviderTests__Set__WhenNotExistingKeyGiven_ThenValueCreated(
		string filepath
	)
	{
		// arrange
		var options = new Dictionary<string, string>
		{
			{ "config-filepath", filepath },
		};
		var key = Guid.NewGuid().ToString("N");
		var value = Guid.NewGuid().ToString("N");
		await _configurationProvider.LoadAsync(options, default);

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
}