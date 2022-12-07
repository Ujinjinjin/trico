using Comar.Extensions;
using System.Runtime.Serialization;

namespace Comar.Serialization;

internal abstract class SerializerBase : ISerializer
{
    /// <inheritdoc />
    public T Deserialize<T>(string contents)
    {
        try
        {
            return TryDeserialize<T>(contents);
        }
        catch (Exception e)
        {
            throw new SerializationException(default, e);
        }
    }
    
    private T TryDeserialize<T>(string contents)
    {
        if (string.IsNullOrWhiteSpace(contents))
        {
            throw new SerializationException();
        }

        var obj = DeserializeObject<T>(contents);
        if (obj is null)
        {
            throw new SerializationException();
        }

        return obj;
    }

    protected abstract T? DeserializeObject<T>(string contents);

    /// <inheritdoc />
    public string Serialize<T>(T data)
    {
        try
        {
            return TrySerialize(data);
        }
        catch (Exception e)
        {
            throw new SerializationException(default, e);
        }
    }

    private string TrySerialize<T>(T data)
    {
        if (data is null)
        {
            throw new SerializationException();
        }

        var s = SerializeObject(data);

        return s.ToUnixEol();
    }
    
    protected abstract string SerializeObject<T>(T data);
}