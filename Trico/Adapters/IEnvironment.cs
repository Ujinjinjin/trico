namespace Trico.Adapters;

/// <summary> Adapter over system environment class </summary>
internal interface IEnvironment
{
	/// <summary> Try getting environment variable </summary>
	/// <param name="variable">Variable name</param>
	bool TryGetEnvironmentVariable(string variable, out string? value);

	/// <summary> Retrieves all environment variable names and their values from the current process </summary>
	IDictionary<string, string> GetEnvironmentVariables();

	/// <summary> Creates, modifies, or deletes an environment variable stored in the current process </summary>
	/// <param name="variable">The name of an environment variable</param>
	/// <param name="value">A value to assign to variable</param>
	void SetEnvironmentVariable(string variable, string value);
}
