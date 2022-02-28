namespace Comar.Configuration;

/// <inheritdoc />
internal class Configuration : IConfiguration
{
	private readonly IReadOnlyList<IConfigurationProvider> _providers;

	public Configuration(IReadOnlyList<IConfigurationProvider> providers)
	{
		_providers = providers ?? throw new ArgumentNullException(nameof(providers));
	}

	/// <inheritdoc />
	public void Dispose()
	{
		foreach (var provider in _providers)
		{
			provider.Dispose();
		}
	}

	/// <inheritdoc />
	public string? this[string key]
	{
		get => GetConfiguration(key);
		set => SetConfiguration(key, value);
	}

	/// <inheritdoc />
	public void Load(IDictionary<string, string> options)
	{
		foreach (var provider in _providers)
		{
			provider.Load(options);
		}
	}

	/// <summary>  </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	private string? GetConfiguration(string key)
	{
		foreach (var provider in _providers)
		{
			if (provider.TryGet(key, out var value))
			{
				return value;
			}
		}

		return default;
	}

	/// <summary>  </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	private void SetConfiguration(string key, string? value)
	{
		foreach (var provider in _providers)
		{
			provider.Set(key, value);
		}
	}
}
