using Trico.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Trico.Extensions;

/// <summary> Service provider extensions </summary>
public static class ServiceProviderExtensions
{
	/// <summary> Configure configuration builder and use configuration </summary>
	/// <param name="sp">Service provider containing configuration builder and providers</param>
	/// <returns>Original service provider</returns>
	/// <exception cref="NullReferenceException">Throws an exception if configuration builder was not registered</exception>
	public static IServiceProvider UseConfiguration(
		this IServiceProvider sp
	)
	{
		var builder = sp.GetService<IConfigurationBuilder>();
		if (builder is null)
		{
			throw new NullReferenceException($"{nameof(IConfigurationBuilder)} service is not registered");
		}

		var providers = sp.GetServices<IConfigurationProvider>();
		foreach (var provider in providers)
		{
			builder.AddProvider(provider);
		}

		return sp;
	}
}
