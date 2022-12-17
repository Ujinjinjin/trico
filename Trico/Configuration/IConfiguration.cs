namespace Trico.Configuration;

/// <summary> Configuration </summary>
public interface IConfiguration
{
	/// <summary> Configuration accessor by key </summary>
	/// <param name="key">Configuration key</param>
	string? this[string key] { get; set; }

	/// <summary> Load configuration from providers </summary>
	/// <param name="options">Options that must be passed to configuration providers</param>
	void Load(IDictionary<string, string> options);

	/// <summary> Load configuration from providers </summary>
	/// <param name="options">Options that must be passed to configuration providers</param>
	/// <param name="ct">Cancellation token</param>
	Task LoadAsync(IDictionary<string, string> options, CancellationToken ct);

	/// <summary> Dump current configuration state </summary>
	void Dump();

	/// <summary> Asynchronously dump current configuration state </summary>
	/// <param name="ct">Cancellation token</param>
	Task DumpAsync(CancellationToken ct);
}
