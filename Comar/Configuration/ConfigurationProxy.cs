namespace Comar.Configuration;

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
	public void Dispose()
	{
		Configuration.Dispose();
	}

	/// <inheritdoc />
	public string? this[string key]
	{
		get => Configuration[key];
		set => Configuration[key] = value;
	}

	/// <inheritdoc />
	public void Load(IDictionary<string, string> options)
	{
		Configuration.Load(options);
	}
}