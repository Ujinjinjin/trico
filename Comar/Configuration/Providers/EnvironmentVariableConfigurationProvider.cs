using Comar.Adapters;
using System.Collections.Concurrent;

namespace Comar.Configuration.Providers;

/// <summary> Long-term configuration provider storing options as environment variables </summary>
internal sealed class EnvironmentVariableConfigurationProvider : IConfigurationProvider
{
	private readonly IEnvironment _environment;
	private string _prefix;
	private readonly IDictionary<string, string> _configurations;

	public EnvironmentVariableConfigurationProvider(IEnvironment environment)
	{
		_environment = environment ?? throw new ArgumentNullException(nameof(environment));
		_prefix = string.Empty;
		_configurations = new ConcurrentDictionary<string, string>();
	}

	/// <inheritdoc />
	public bool TryGet(string key, out string? value)
	{
		return _configurations.TryGetValue(key, out value);
	}

	/// <inheritdoc />
	public void Set(string key, string value)
	{
		if (_configurations.ContainsKey(key))
		{
			_configurations[key] = value;
		}
		else
		{
			_configurations.Add(key, value);
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
		_configurations.Clear();

		if (options.TryGetValue("prefix", out var prefix) && !string.IsNullOrWhiteSpace(prefix))
		{
			_prefix = prefix;
		}

		var envVars = _environment.GetEnvironmentVariables();

		foreach (var (variable, value) in envVars)
		{
			if (string.IsNullOrWhiteSpace(_prefix) || variable.StartsWith(_prefix))
			{
				_configurations.Add(variable.Remove(0, _prefix.Length), value);
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
		foreach (var option in _configurations)
		{
			_environment.SetEnvironmentVariable($"{_prefix}{option.Key}", option.Value);
		}

		return Task.CompletedTask;
	}
}
