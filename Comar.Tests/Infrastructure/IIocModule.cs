using System;

namespace Comar.Tests.Infrastructure;

/// <summary> Application's IoC module </summary>
public interface IIocModule
{
	/// <summary> Build module </summary>
	/// <returns>Service provider</returns>
	IServiceProvider Build();
}
