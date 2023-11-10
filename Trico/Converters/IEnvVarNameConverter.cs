namespace Trico.Converters;

public interface IEnvVarNameConverter
{
	string ToConfigKey(string envVarName, string? prefix);
	string ToEnvVarName(string configKey, string? prefix);
}
