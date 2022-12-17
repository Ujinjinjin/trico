using Trico.Adapters;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Trico.Tests.Adapters;

public class FileIoTests : UnitTestBase
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public async Task FileIoTests_ReadAsync__WhenNullOrEmptyFilepathGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var fileIo = new FileIo();
		var action = async () => await fileIo.ReadAsync(filepath, default);

		// act & assert
		await action.Should().ThrowAsync<ArgumentNullException>();
	}

	[Fact]
	public async Task FileIoTests_ReadAsync__WhenValidFilepathGiven_ThenFileContentsReturned()
	{
		// arrange
		var fileIo = new FileIo();
		var expected = "test content\n";
		var filepath = "./InputData/text-file-read.txt";

		// act
		var result = await fileIo.ReadAsync(filepath, default);

		// assert
		result.Should().Be(expected);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData(" ")]
	public async Task FileIoTests_WriteAsync__WhenNullOrEmptyFilepathGiven_ThenExceptionThrown(string filepath)
	{
		// arrange
		var fileIo = new FileIo();
		var content = Fixture.Create<StringBuilder>();
		var action = async () => await fileIo.WriteAsync(filepath, content, default);

		// act & assert
		await action.Should().ThrowAsync<ArgumentNullException>();
	}

	[Fact]
	public async Task FileIoTests_WriteAsync__WhenValidFilepathAndContentGiven_ThenContentWrittenToFile()
	{
		// arrange
		var fileIo = new FileIo();
		var contentString = Fixture.Create<string>();
		var content = new StringBuilder(contentString);
		var filepath = "./InputData/text-file-write.txt";

		// act
		await fileIo.WriteAsync(filepath, content, default);
		var result = await fileIo.ReadAsync(filepath, default);

		// assert
		result.Should().Be(content.ToString());
	}
}
