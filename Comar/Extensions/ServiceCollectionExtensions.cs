using Comar.Configuration;
using Comar.Configuration.Providers;
using Comar.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Comar.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddConfiguration(this IServiceCollection sc)
	{
		sc.AddScoped<ISerializerFactory, SerializerFactory>();
		sc.AddScoped<IConfigurationBuilder, ConfigurationBuilder>();
		sc.AddScoped<IConfiguration, Configuration.Configuration>();

		return sc;
	}

	public static IServiceCollection AddFileProvider(this IServiceCollection sc)
	{
		sc.AddScoped<IConfigurationProvider, FileConfigurationProvider>();

		return sc;
	}

	public static IServiceCollection AddInMemoryProvider(
		this IServiceCollection sc,
		IDictionary<string, string?>? options = default
	)
	{
		sc.AddScoped<IConfigurationProvider>(x => new InMemoryConfigurationProvider(options));

		return sc;
	}
}