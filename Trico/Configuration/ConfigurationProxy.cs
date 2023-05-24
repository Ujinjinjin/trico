namespace Trico.Configuration;

/// <inheritdoc />
internal sealed class ConfigurationProxy : IConfiguration
{
	private readonly IConfigurationBuilder _configurationBuilder;

	private IConfiguration? _configuration;
	private IConfiguration Configuration
	{
		get
		{
			_configuration ??= _configurationBuilder.Build();
			return _configuration;
		}
	}

	public ConfigurationProxy(IConfigurationBuilder configurationBuilder)
	{
		_configurationBuilder = configurationBuilder ?? throw new ArgumentNullException(nameof(configurationBuilder));
		_configuration = default;
	}

	/// <inheritdoc />
	public string? this[string key]
	{
		get => Configuration[key];
		set => Configuration[key] = value;
	}

	/// <inheritdoc />
	public string Get(string key)
	{
		return Configuration.Get(key);
	}

	/// <inheritdoc />
	public bool TryGet(string key, out string? value)
	{
		return Configuration.TryGet(key, out value);
	}

	/// <inheritdoc />
	public void Load(IDictionary<string, string> options)
	{
		Configuration.Load(options);
	}

	/// <inheritdoc />
	public async Task LoadAsync(IDictionary<string, string> options, CancellationToken ct)
	{
		await Configuration.LoadAsync(options, ct);
	}

	/// <inheritdoc />
	public void Dump()
	{
		Configuration.Dump();
	}

	/// <inheritdoc />
	public async Task DumpAsync(CancellationToken ct)
	{
		await Configuration.DumpAsync(ct);
	}
}
