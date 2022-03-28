namespace Comar.Configuration;

/// <summary> Configuration builder </summary>
public interface IConfigurationBuilder
{
	/// <summary> Add provider to the list </summary>
	/// <param name="provider">Configuration provider</param>
	/// <returns>Current configuration builder</returns>
	IConfigurationBuilder AddProvider(IConfigurationProvider provider);

	/// <summary> Build configuration object with all registered providers </summary>
	/// <returns>Configuration object</returns>
	IConfiguration Build();
}
