namespace Trico.Configuration;

/// <inheritdoc />
internal sealed class ConfigurationBuilder : IConfigurationBuilder
{
	private readonly IList<IConfigurationProvider> _providers;

	public ConfigurationBuilder()
	{
		_providers = new List<IConfigurationProvider>();
	}

	/// <inheritdoc />
	public IConfigurationBuilder AddProvider(IConfigurationProvider provider)
	{
		if (!_providers.Contains(provider))
		{
			_providers.Add(provider);
		}

		return this;
	}

	/// <inheritdoc />
	public IConfiguration Build()
	{
		return new Configuration((IReadOnlyList<IConfigurationProvider>)_providers);
	}
}
