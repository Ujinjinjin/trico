namespace Comar.Configuration;

/// <summary>  </summary>
public interface IConfigurationBuilder
{
	/// <summary>  </summary>
	/// <param name="provider"></param>
	/// <returns></returns>
	IConfigurationBuilder AddProvider(IConfigurationProvider provider);

	/// <summary>  </summary>
	/// <returns></returns>
	IConfiguration Build();
}
