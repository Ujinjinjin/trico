using Comar.Extensions;

namespace Comar.Tests.Extensions;

public class StringExtensionsTests : UnitTestBase
{
	[Fact]
	public void StringExtensionsTests__ToUnixEol__WhenStringContainingWindowsEolGiven_ThenEolReplacedWithUnix()
	{
		// Arrange
		var initialString = "\n\r\n\r";
		var expected = "\n\n\r";

		// Act
		var result = initialString.ToUnixEol();

		// Assert
		result.Should().Be(expected);
	}
}
