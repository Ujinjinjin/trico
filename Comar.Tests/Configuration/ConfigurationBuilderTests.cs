using Comar.Configuration;
using System.Collections.Generic;

namespace Comar.Tests.Configuration;

public class ConfigurationBuilderTests : UnitTestBase
{
	[Fact]
	public void ConfigurationBuilderTests_Smoke()
	{
		// arrange
		var builder = new ConfigurationBuilder();
		foreach (var provider in CreateMockProviders(Fixture.Create<int>()))
		{
			builder.AddProvider(provider);
		}

		// act
		var configuration = builder.Build();

		// assert
		configuration.Should().NotBeNull();
	}

	private IEnumerable<IConfigurationProvider> CreateMockProviders(int count)
	{
		for (var i = 0; i < count; i++)
		{
			yield return new Mock<IConfigurationProvider>().Object;
		}
	}
}
