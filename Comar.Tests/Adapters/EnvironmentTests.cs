namespace Comar.Tests.Adapters;

public class EnvironmentTests : UnitTestBase
{
	[Fact]
	public void EnvironmentTests_TryGetEnvironmentVariable__WhenNotExistingVariableNameGiven_ThenValueNotReturned()
	{
		// arrange
		var variable = Fixture.Create<string>();
		var environment = new Comar.Adapters.Environment();

		// act
		var result = environment.TryGetEnvironmentVariable(variable, out var value);

		// assert
		result.Should().BeFalse();
		value.Should().BeNull();
	}

	[Fact]
	public void EnvironmentTests_TryGetEnvironmentVariable__WhenExistingVariableNameGiven_ThenValueReturned()
	{
		// arrange
		var variable = Fixture.Create<string>();
		var expected = Fixture.Create<string>();
		var environment = new Comar.Adapters.Environment();
		environment.SetEnvironmentVariable(variable, expected);

		// act
		var result = environment.TryGetEnvironmentVariable(variable, out var value);

		// assert
		result.Should().BeTrue();
		value.Should().Be(expected);
	}

	[Fact]
	public void EnvironmentTests_GetEnvironmentVariables__WhenExistingVariableNameGiven_ThenValueReturned()
	{
		// arrange
		var variable = Fixture.Create<string>();
		var expected = Fixture.Create<string>();
		var environment = new Comar.Adapters.Environment();
		environment.SetEnvironmentVariable(variable, expected);

		// act
		var variables = environment.GetEnvironmentVariables();
		var actual = variables.TryGetValue(variable, out var value);

		// assert
		variables.Should().ContainKey(variable);
		variables.Should().ContainValue(expected);
		actual.Should().BeTrue();
		value.Should().Be(expected);
	}

	[Fact]
	public void EnvironmentTests_GetEnvironmentVariables__Smoke()
	{
		// arrange
		var environment = new Comar.Adapters.Environment();

		// act
		var variables = environment.GetEnvironmentVariables();

		// assert
		variables.Should().NotBeEmpty();
	}
}
