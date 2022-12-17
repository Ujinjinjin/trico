namespace Trico.Serialization;

/// <summary> Internal serializer factory </summary>
public interface ISerializerFactory
{
	/// <summary> Create internal serializer suitable for specified file </summary>
	/// <param name="filepath">Path to the file</param>
	/// <returns>Internal serializer</returns>
	ISerializer CreateSerializer(string filepath);
}
