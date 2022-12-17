using Comar.Configuration;
using System;
using System.Collections.Generic;
using System.Data;

namespace Comar.Tests.Configuration;

public class ConfigurationTests : UnitTestBase
{
	[Fact]
	public void ConfigurationTests_Get__WhenNotExistingKeyGiven_ThenNullReturned()
	{
		// arrange
		var providers = CreateMockProviders(Fixture.Create<int>());
		var configuration = new Comar.Configuration.Configuration(providers);
		var key = Fixture.Create<string>();

		// act
		var result = configuration[key];

		// assert
		result.Should().BeNull();
	}

	[Fact]
	public void ConfigurationTests_Get__WhenExistingKeyGiven_ThenValueReturned()
	{
		// arrange
		var providers = CreateMockProviders(Fixture.Create<int>());
		var configuration = new Comar.Configuration.Configuration(providers);
		var key = Fixture.Create<string>();
		var value = Fixture.Create<string>();

		configuration[key] = value;

		// act
		var result = configuration[key];

		// assert
		result.Should().Be(value);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void ConfigurationTests_Set__WhenNullOrEmptyValueGiven_ThenExceptionThrown(string value)
	{
		// arrange
		var providers = CreateMockProviders(Fixture.Create<int>());
		var configuration = new Comar.Configuration.Configuration(providers);
		var key = Fixture.Create<string>();
		var action = () => configuration[key] = value;

		// act & assert
		action.Should().Throw<NoNullAllowedException>();
	}

	private IReadOnlyList<IConfigurationProvider> CreateMockProviders(int count)
	{
		var providers = new IConfigurationProvider[count];
		for (var i = 0; i < count; i++)
		{
			var mockProvider = new Mock<IConfigurationProvider>();
			var holder = new Holder();

			mockProvider
				.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<string>()))
				.Callback(new Action<string, string>((key, value) => holder.Set(key, value)));
			mockProvider
				.Setup(x => x.TryGet(It.IsAny<string>(), out It.Ref<string?>.IsAny))
				.Returns(
					(string key, out string? value) =>
					{
						value = holder.Get(key);
						return !string.IsNullOrWhiteSpace(value);
					}
				);

			providers[i] = mockProvider.Object;
		}

		return providers;
	}

	private class Holder
	{
		private readonly Dictionary<string, string> _collection;

		public Holder()
		{
			_collection = new Dictionary<string, string>();
		}

		public string? Get(string key)
		{
			return _collection.TryGetValue(key, out var result)
				? result
				: default;
		}

		public void Set(string key, string value)
		{
			if (_collection.ContainsKey(key))
			{
				_collection[key] = value;
			}
			else
			{
				_collection.Add(key, value);
			}
		}
	}
}
