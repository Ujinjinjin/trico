﻿using Trico.Adapters;
using Trico.Configuration;
using Trico.Configuration.Providers;
using Trico.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Trico.Converters;

namespace Trico.Extensions;

/// <summary> Service collection extensions </summary>
public static class ServiceCollectionExtensions
{
	/// <summary> Register configuration dependencies </summary>
	/// <param name="sc">Service collection to which configurations dependencies will be registered </param>
	/// <returns>Original service collection</returns>
	public static IServiceCollection AddConfiguration(this IServiceCollection sc)
	{
		// configuration
		sc.AddSingleton<IConfigurationBuilder, ConfigurationBuilder>();
		sc.AddSingleton<IConfiguration, ConfigurationProxy>();

		return sc;
	}

	/// <summary> Register environment variable configuration provider in given service collection </summary>
	/// <param name="sc">Service collection to which provider will be added</param>
	/// <returns>Original service collection</returns>
	public static IServiceCollection AddEnvironmentVariableProvider(this IServiceCollection sc)
	{
		sc.AddSingleton<IEnvironment, Adapters.Environment>();
		sc.AddSingleton<IEnvVarNameConverter, EnvVarNameConverter>();
		sc.AddSingleton<IConfigurationProvider, EnvironmentVariableConfigurationProvider>();

		return sc;
	}

	/// <summary> Register file configuration provider in given service collection </summary>
	/// <param name="sc">Service collection to which provider will be added</param>
	/// <returns>Original service collection</returns>
	public static IServiceCollection AddFileProvider(this IServiceCollection sc)
	{
		sc.AddSingleton<IFileIo, FileIo>();
		sc.AddSingleton<ISerializerFactory, SerializerFactory>();
		sc.AddSingleton<IConfigurationProvider, FileConfigurationProvider>();

		return sc;
	}

	/// <summary> Register in-memory configuration provider in given service collection </summary>
	/// <param name="sc">Service collection to which provider will be added</param>
	/// <param name="options">Additional options that will be passed into in-memory configuration provider</param>
	/// <returns>Original service collection</returns>
	public static IServiceCollection AddInMemoryProvider(
		this IServiceCollection sc,
		IDictionary<string, string>? options = default
	)
	{
		sc.AddSingleton<IConfigurationProvider>(_ => new InMemoryConfigurationProvider(options));

		return sc;
	}
}
