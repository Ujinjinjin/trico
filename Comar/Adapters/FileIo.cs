using System.Text;

namespace Comar.Adapters;

/// <inheritdoc />
internal sealed class FileIo : IFileIo
{
	/// <inheritdoc />
	public async Task<string> ReadAsync(string filepath, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(filepath))
		{
			throw new ArgumentNullException(nameof(filepath));
		}

		return await File.ReadAllTextAsync(filepath, ct);
	}

	/// <inheritdoc />
	public async Task WriteAsync(string filepath, StringBuilder content, CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(filepath))
		{
			throw new ArgumentNullException(nameof(filepath));
		}

		await using var streamWriter = new StreamWriter(filepath);
		await streamWriter.WriteAsync(content, ct);
	}
}
