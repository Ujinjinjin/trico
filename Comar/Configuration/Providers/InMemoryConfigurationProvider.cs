using System.Collections.Concurrent;

namespace Comar.Configuration.Providers;

/// <summary> Short-term configuration provider storing options in memory </summary>
internal sealed class InMemoryConfigurationProvider : IConfigurationProvider
{
	private readonly IDictionary<string, string> _options;

	public InMemoryConfigurationProvider(IDictionary<string, string>? options)
	{
		_options = options ?? new ConcurrentDictionary<string, string>();
	}

	/// <inheritdoc />
	public bool TryGet(string key, out string? value)
	{
		return _options.TryGetValue(key, out value);
	}

	/// <inheritdoc />
	public void Set(string key, string value)
	{
		if (_options.ContainsKey(key))
		{
			_options[key] = value;
		}
		else
		{
			_options.Add(key, value);
		}
	}

	/// <inheritdoc />
	void IConfigurationProvider.Load(IDictionary<string, string> options)
	{
		((IConfigurationProvider)this).LoadAsync(options, default).GetAwaiter().GetResult();
	}

	/// <inheritdoc />
	Task IConfigurationProvider.LoadAsync(IDictionary<string, string> options, CancellationToken ct)
	{
		_options.Clear();

		foreach (var (key, value) in options)
		{
			_options.Add(key, value);
		}

		return Task.CompletedTask;
	}

	/// <inheritdoc />
	void IConfigurationProvider.Dump()
	{
		return;
	}

	/// <inheritdoc />
	Task IConfigurationProvider.DumpAsync(CancellationToken ct)
	{
		return Task.CompletedTask;
	}
}
