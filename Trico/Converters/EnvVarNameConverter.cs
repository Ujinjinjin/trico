namespace Trico.Converters;

public class EnvVarNameConverter : IEnvVarNameConverter
{
	public string ToConfigKey(string envVarName, string? prefix)
	{
		if (string.IsNullOrWhiteSpace(envVarName))
		{
			throw new ArgumentNullException(nameof(envVarName));
		}

		if (!string.IsNullOrWhiteSpace(prefix))
		{
			envVarName = envVarName.Remove(0, prefix.Length);
		}

		return envVarName
			.Replace("__", ".")
			.Replace("_", "-")
			.ToLower();
	}

	public string ToEnvVarName(string configKey, string? prefix)
	{
		if (string.IsNullOrWhiteSpace(configKey))
		{
			throw new ArgumentNullException(nameof(configKey));
		}

		prefix ??= string.Empty;
		return prefix + configKey
			.Replace(".", "__")
			.Replace("-", "_")
			.ToUpper();
	}
}
