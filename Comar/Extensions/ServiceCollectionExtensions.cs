using Comar.Configuration;
using Comar.Configuration.Providers;
using Comar.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace Comar.Extensions;

/// <summary> Service collection extensions </summary>
public static class ServiceCollectionExtensions
{
	/// <summary> Register configuration dependencies </summary>
	/// <param name="sc">Service collection where configurations dependencies will be registered </param>
	/// <returns>Original service collection</returns>
	public static IServiceCollection AddConfiguration(this IServiceCollection sc)
	{
		sc.AddScoped<ISerializerFactory, SerializerFactory>();
		sc.AddScoped<IConfigurationBuilder, ConfigurationBuilder>();
		sc.AddScoped<IConfiguration, ConfigurationProxy>();

		return sc;
	}

	/// <summary> Register file configuration provider in given service collection </summary>
	/// <param name="sc">Service collection where provider will be added</param>
	/// <returns>Original service collection</returns>
	public static IServiceCollection AddFileProvider(this IServiceCollection sc)
	{
		sc.AddScoped<IConfigurationProvider, FileConfigurationProvider>();

		return sc;
	}

	/// <summary> Register in-memory configuration provider in given service collection </summary>
	/// <param name="sc">Service collection where provider will be added</param>
	/// <param name="options">Additional options that will be passed into in-memory configuration provider</param>
	/// <returns>Original service collection</returns>
	public static IServiceCollection AddInMemoryProvider(
		this IServiceCollection sc,
		IDictionary<string, string?>? options = default
	)
	{
		sc.AddScoped<IConfigurationProvider>(x => new InMemoryConfigurationProvider(options));

		return sc;
	}
}