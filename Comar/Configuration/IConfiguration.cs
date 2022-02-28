namespace Comar.Configuration;

/// <summary>  </summary>
public interface IConfiguration : IDisposable
{
	/// <summary>  </summary>
	/// <param name="key"></param>
	string? this[string key] { get; set; }

	/// <summary>  </summary>
	/// <param name="options"></param>
	void Load(IDictionary<string, string> options);
}
