using Comar.Serializers;

namespace Comar.Factories;

/// <summary> Internal serializer factory </summary>
public interface ISerializerFactory
{
	/// <summary> Create internal serializer suitable for specified file </summary>
	/// <param name="filename">Filename</param>
	/// <returns>Internal serializer</returns>
	ISerializer CreateSerializer(string filename);
}