using Comar.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Comar.Extensions;

public static class ServiceProviderExtensions
{
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