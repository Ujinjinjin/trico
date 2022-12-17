namespace Trico.Extensions;

/// <summary> Collection of extensions for <see cref="string"/> </summary>
internal static class StringExtensions
{
	/// <summary> Replace all `\r\n` with `\n` in given string </summary>
	/// <param name="source">Source string</param>
	public static string ToUnixEol(this string source)
	{
		return source.Replace("\r\n", "\n");
	}
}
