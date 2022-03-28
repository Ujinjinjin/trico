namespace Comar.Configuration;

/// <summary> Configuration provider </summary>
public interface IConfigurationProvider : IDisposable, IAsyncDisposable
{
	/// <summary> Try to get value by its key </summary>
	/// <param name="key">Configuration key</param>
	/// <param name="value">Configuration out value</param>
	/// <returns>True if configuration key exists false otherwise</returns>
	bool TryGet(string key, out string? value);

	/// <summary> Set configuration value by its key </summary>
	/// <param name="key">Configuration key</param>
	/// <param name="value">Configuration value</param>
	void Set(string key, string? value);

	/// <summary> Load configuration </summary>
	/// <param name="options">Options</param>
	internal void Load(IDictionary<string, string> options);

	/// <summary> Asynchronously load configuration </summary>
	/// <param name="options">Options</param>
	/// <param name="ct">Cancellation token</param>
	internal Task LoadAsync(IDictionary<string, string> options, CancellationToken ct);

	/// <summary> Dump current configuration state </summary>
	internal void Dump();

	/// <summary> Asynchronously dump current configuration state </summary>
	internal Task DumpAsync(CancellationToken ct);
}
