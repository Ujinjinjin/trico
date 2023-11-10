using System;
using Trico.Converters;

namespace Trico.Tests.Converters;

public class EnvVarNameConverterTests : UnitTestBase
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void EnvVarNameConverterTests_ToConfigKey__WhenEmptyEnvVarNameGiven_ThenErrorThrown(string envVarName)
	{
		// arrange
		var converter = new EnvVarNameConverter();

		// act
		var func = () => converter.ToConfigKey(envVarName, Fixture.Create<string>());

		// assert
		func.Should().Throw<ArgumentNullException>();
	}

	[Theory]
	[InlineData("ENV_VAR", "env-var")]
	[InlineData("ENV_VAR_1", "env-var-1")]
	[InlineData("ENV_VAR_WITH_LONG_NAME", "env-var-with-long-name")]
	[InlineData("NESTED__ENV_VAR", "nested.env-var")]
	public void EnvVarNameConverterTests_ToConfigKey__WhenEnvVarNameGiven_ThenItConvertedIntoConfigKey(string envVarName, string expected)
	{
		// arrange
		var converter = new EnvVarNameConverter();
		var prefix = Fixture.Create<string>();

		envVarName = prefix + envVarName;

		// act
		var configKey = converter.ToConfigKey(envVarName, prefix);

		// assert
		configKey.Should().Be(expected);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public void EnvVarNameConverterTests_ToEnvVarName__WhenEmptyConfigKeyGiven_ThenErrorThrown(string envVarName)
	{
		// arrange
		var converter = new EnvVarNameConverter();

		// act
		var func = () => converter.ToEnvVarName(envVarName, Fixture.Create<string>());

		// assert
		func.Should().Throw<ArgumentNullException>();
	}

	[Theory]
	[InlineData("env-var", "ENV_VAR")]
	[InlineData("env-var-1", "ENV_VAR_1")]
	[InlineData("env-var-with-long-name", "ENV_VAR_WITH_LONG_NAME")]
	[InlineData("nested.env-var", "NESTED__ENV_VAR")]
	public void EnvVarNameConverterTests_ToEnvVarName__WhenConfigKeyGiven_ThenItConvertedIntoEnvVarName(string configKey, string expected)
	{
		// arrange
		var converter = new EnvVarNameConverter();
		var prefix = Fixture.Create<string>();
		expected = prefix + expected;

		// act
		var envVarName = converter.ToEnvVarName(configKey, prefix);

		// assert
		envVarName.Should().Be(expected);
	}
}
