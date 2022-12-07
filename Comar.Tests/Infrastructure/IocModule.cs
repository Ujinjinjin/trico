using Comar.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Comar.Tests.Infrastructure;

/// <inheritdoc />
public class IocModule : IIocModule
{
	/// <inheritdoc />
	public IServiceProvider Build()
	{
		var collection = new ServiceCollection();

		collection.AddConfiguration()
			.AddEnvironmentVariableProvider()
			.AddFileProvider()
			.AddInMemoryProvider(GetDefaultConfiguration());

		return collection.BuildServiceProvider();
	}

	private IDictionary<string, string> GetDefaultConfiguration()
	{
		var configuration = new ConcurrentDictionary<string, string>();

		configuration.TryAdd("property-1", "value-1");
		configuration.TryAdd("property-2", "value-2");
		configuration.TryAdd("property-3", "value-3");
		configuration.TryAdd("property-4", "value-4");
		configuration.TryAdd("property-5", "value-5");

		return configuration;
	}
}