namespace Comar.Configuration;

/// <summary>  </summary>
public interface IConfigurationProvider : IDisposable, IAsyncDisposable
{
	/// <summary>  </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	bool TryGet(string key, out string? value);

	/// <summary>  </summary>
	/// <param name="key"></param>
	/// <param name="value"></param>
	void Set(string key, string? value);

	/// <summary>  </summary>
	/// <param name="options"></param>
	internal void Load(IDictionary<string, string> options);

	/// <summary>  </summary>
	/// <param name="options"></param>
	/// <param name="ct"></param>
	internal Task LoadAsync(IDictionary<string, string> options, CancellationToken ct);

	/// <summary>  </summary>
	internal void Dump();

	/// <summary>  </summary>
	internal Task DumpAsync(CancellationToken ct);
}
