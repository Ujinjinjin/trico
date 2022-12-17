using System.Text;

namespace Trico.Adapters;

/// <summary> Abstraction over simple IO operations </summary>
internal interface IFileIo
{
	/// <summary> Read all text in file </summary>
	/// <param name="filepath">The complete file path to read from. path can be a file name</param>
	/// <param name="ct">Cancellation token</param>
	Task<string> ReadAsync(string filepath, CancellationToken ct);

	/// <summary> Write to file </summary>
	/// <param name="filepath">The complete file path to write to. path can be a file name</param>
	/// <param name="content">The string, as a string builder, to write to the file</param>
	/// <param name="ct">Cancellation token</param>
	/// <returns></returns>
	Task WriteAsync(string filepath, StringBuilder content, CancellationToken ct);
}
