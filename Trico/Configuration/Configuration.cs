using System.Data;

namespace Trico.Configuration;

/// <inheritdoc />
internal sealed class Configuration : IConfiguration
{
	private readonly IReadOnlyList<IConfigurationProvider> _providers;

	internal Configuration(IReadOnlyList<IConfigurationProvider> providers)
	{
		_providers = providers ?? throw new ArgumentNullException(nameof(providers));
	}

	public string? this[string key]
	{
		get => GetConfiguration(key);
		set => SetConfiguration(key, value);
	}

	/// <inheritdoc />
	public void Load(IDictionary<string, string> options)
	{
		LoadAsync(options, default).GetAwaiter().GetResult();
	}

	/// <inheritdoc />
	public async Task LoadAsync(IDictionary<string, string> options, CancellationToken ct)
	{
		foreach (var provider in _providers)
		{
			await provider.LoadAsync(options, ct);
		}
	}

	/// <inheritdoc />
	public void Dump()
	{
		DumpAsync(default).GetAwaiter().GetResult();
	}

	/// <inheritdoc />
	public async Task DumpAsync(CancellationToken ct)
	{
		foreach (var provider in _providers)
		{
			await provider.DumpAsync(ct);
		}
	}

	/// <summary> Get configuration by key from the first available provider containing given key </summary>
	/// <param name="key">Key to access configuration</param>
	/// <returns>Configuration value</returns>
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

	/// <summary> Set configuration value by given key in all providers </summary>
	/// <param name="key">Key to update configuration</param>
	/// <param name="value">Value to set</param>
	private void SetConfiguration(string key, string? value)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			throw new NoNullAllowedException(nameof(value));
		}

		foreach (var provider in _providers)
		{
			provider.Set(key, value);
		}
	}
}
