using Comar.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Comar.Tests.Infrastructure;

/// <inheritdoc />
public class IocModule : IIocModule
{
	/// <inheritdoc />
	public IServiceProvider Build()
	{
		var collection = new ServiceCollection();

		collection.AddConfiguration()
			.AddFileProvider()
			.AddInMemoryProvider();

		return collection.BuildServiceProvider();
	}
}