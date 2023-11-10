using Trico.Adapters;
using System.Collections.Concurrent;
using Trico.Converters;

namespace Trico.Configuration.Providers;

/// <summary> Long-term configuration provider storing options as environment variables </summary>
internal sealed class EnvironmentVariableConfigurationProvider : IConfigurationProvider
{
	private readonly IEnvironment _environment;
	private readonly IEnvVarNameConverter _envVarNameConverter;
	private string _prefix;
	private readonly IDictionary<string, string> _configurations;

	public EnvironmentVariableConfigurationProvider(
		IEnvironment environment,
		IEnvVarNameConverter envVarNameConverter
	)
	{
		_environment = environment ?? throw new ArgumentNullException(nameof(environment));
		_envVarNameConverter = envVarNameConverter ?? throw new ArgumentNullException(nameof(envVarNameConverter));
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
		_configurations[key] = value;
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
			if (!variable.StartsWith(_prefix))
			{
				continue;
			}

			var key = _envVarNameConverter.ToConfigKey(variable, _prefix);
			_configurations.Add(key, value);
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
			var envVarName = _envVarNameConverter.ToEnvVarName(option.Key, _prefix);
			_environment.SetEnvironmentVariable(envVarName, option.Value);
		}

		return Task.CompletedTask;
	}
}
