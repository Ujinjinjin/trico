namespace Comar.Extensions;

/// <summary> Collection of extensions for <see cref="IList{T}"/> </summary>
internal static class ListExtensions
{
	/// <summary> Select sequence from list of item beginning at startIndex and ending at endIndex </summary>
	/// <param name="source">Source list of items</param>
	/// <param name="startIndex">Start index</param>
	/// <param name="endIndex">End index</param>
	/// <typeparam name="T">The type of items in the list</typeparam>
	/// <returns>Sequence of items</returns>
	public static IList<T> Sequence<T>(this IList<T> source, Index startIndex, Index endIndex)
	{
		if (endIndex.IsFromEnd)
		{
			endIndex = source.Count - endIndex.Value;
		}

		var targetLength = endIndex.Value - startIndex.Value;
		var target = new T[targetLength];

		for (var i = 0; i < targetLength; i++)
		{
			target[i] = source[startIndex.Value + i];
		}

		return target;
	}
}
