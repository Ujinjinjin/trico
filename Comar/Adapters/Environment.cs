using System.Collections;

namespace Comar.Adapters;

/// <inheritdoc />
internal class Environment : IEnvironment
{
	/// <inheritdoc />
	public bool TryGetEnvironmentVariable(string variable, out string? value)
	{
		value = System.Environment.GetEnvironmentVariable(variable);
		return !string.IsNullOrEmpty(value);
	}

	/// <inheritdoc />
	public IDictionary<string, string> GetEnvironmentVariables()
	{
		var variables = new Dictionary<string, string>();
		foreach (DictionaryEntry de in System.Environment.GetEnvironmentVariables())
		{
			if (de is { Key: string key, Value: string value })
			{
				variables.Add(key, value);
			}
		}

		return variables;
	}

	/// <inheritdoc />
	public void SetEnvironmentVariable(string variable, string value)
	{
		System.Environment.SetEnvironmentVariable(variable, value);
	}
}
