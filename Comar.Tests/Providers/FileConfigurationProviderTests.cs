using Comar.Configuration;
using Comar.Configuration.Providers;
using Comar.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
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
}