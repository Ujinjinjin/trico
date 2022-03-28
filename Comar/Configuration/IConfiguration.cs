namespace Comar.Configuration;

/// <summary> Configuration </summary>
public interface IConfiguration : IDisposable
{
	/// <summary> Configuration accessor by key </summary>
	/// <param name="key">Configuration key</param>
	string? this[string key] { get; set; }

	/// <summary> Load configuration from providers </summary>
	/// <param name="options">Options that must be passed to configuration providers</param>
	void Load(IDictionary<string, string> options);
}
