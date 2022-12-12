using Comar.Adapters;
using System.Collections.Concurrent;

namespace Comar.Configuration.Providers;

/// <summary> Long-term configuration provider storing options as environment variables </summary>
internal sealed class EnvironmentVariableConfigurationProvider : IConfigurationProvider
{
	private readonly IEnvironment _environment;
	private string _prefix;
	private readonly IDictionary<string, string> _options;

	public EnvironmentVariableConfigurationProvider(IEnvironment environment)
	{
		_environment = environment ?? throw new ArgumentNullException(nameof(environment));
		_prefix = string.Empty;
		_options = new ConcurrentDictionary<string, string>();
	}

	/// <inheritdoc />
	public void Dispose()
	{
		((IConfigurationProvider)this).Dump();
		_options.Clear();
	}

	/// <inheritdoc />
	public ValueTask DisposeAsync()
	{
		try
		{
			Dispose();
			return default;
		}
		catch (Exception exc)
		{
			return ValueTask.FromException(exc);
		}
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

		if (options.TryGetValue("prefix", out var prefix) && !string.IsNullOrWhiteSpace(prefix))
		{
			_prefix = prefix;
		}

		var envVars = _environment.GetEnvironmentVariables();

		foreach (var envVar in envVars)
		{
			if (string.IsNullOrWhiteSpace(_prefix) || envVar.Value.StartsWith(_prefix))
			{
				_options.Add(envVar.Key.Remove(0, _prefix.Length), envVar.Value);
			}
		}

		return Task.CompletedTask;
	}

	/// <inheritdoc />
	void IConfigurationProvider.Dump()
	{
		((IConfigurationProvider)this).DumpAsync(default).GetAwaiter().GetResult();
	}

	/// <inheritdoc />
	Task IConfigurationProvider.DumpAsync(CancellationToken ct)
	{
		foreach (var option in _options)
		{
			_environment.SetEnvironmentVariable($"{_prefix}{option.Key}", option.Value);
		}

		return Task.CompletedTask;
	}
}
