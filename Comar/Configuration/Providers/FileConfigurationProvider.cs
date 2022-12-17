using Comar.Adapters;
using Comar.Containers;
using Comar.Serialization;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Text;

namespace Comar.Configuration.Providers;

/// <summary> Long-term configuration provider storing options in a file </summary>
internal sealed class FileConfigurationProvider : IConfigurationProvider
{
	private readonly ISerializerFactory _serializerFactory;
	private readonly IFileIo _fileIo;
	private JsonObject _jObj;
	private string? _filepath;

	public FileConfigurationProvider(
		ISerializerFactory serializerFactory,
		IFileIo fileIo
	)
	{
		_jObj = new JsonObject();
		_serializerFactory = serializerFactory ?? throw new ArgumentNullException(nameof(serializerFactory));
		_fileIo = fileIo ?? throw new ArgumentNullException(nameof(fileIo));
	}

	/// <inheritdoc />
	public bool TryGet(string key, out string? value)
	{
		return _jObj.TryGet(key, out value);
	}

	/// <inheritdoc />
	public void Set(string key, string value)
	{
		_jObj.Set(key, value);
	}

	/// <inheritdoc />
	void IConfigurationProvider.Load(IDictionary<string, string> options)
	{
		((IConfigurationProvider)this).LoadAsync(options, default).GetAwaiter().GetResult();
	}

	/// <inheritdoc />
	async Task IConfigurationProvider.LoadAsync(IDictionary<string, string> options, CancellationToken ct)
	{
		if (!options.TryGetValue("config-filepath", out var filepath))
		{
			throw new KeyNotFoundException($"\"config-filepath\" option must be presented in {nameof(options)} dictionary");
		}

		var serializer = _serializerFactory.CreateSerializer(filepath);

		object? obj;
		try
		{
			var fileContents = await _fileIo.ReadAsync(filepath, ct);

			obj = serializer.Deserialize<object>(fileContents);
		}
		catch (SerializationException e)
		{
			throw new FileLoadException("config file wasn't loaded correctly", e);
		}
		catch (FileNotFoundException)
		{
			obj = default;
		}

		_jObj = new JsonObject(obj);
		_filepath = filepath;
	}

	/// <inheritdoc />
	void IConfigurationProvider.Dump()
	{
		((IConfigurationProvider)this).DumpAsync(default).GetAwaiter().GetResult();
	}

	/// <inheritdoc />
	async Task IConfigurationProvider.DumpAsync(CancellationToken ct)
	{
		if (string.IsNullOrWhiteSpace(_filepath) || _jObj is null)
		{
			throw new FileLoadException("cannot dump file: config file wasn't loaded correctly");
		}

		var serializer = _serializerFactory.CreateSerializer(_filepath);

		var obj = _jObj.ToObject<ExpandoObject>();

		var stringBuilder = new StringBuilder();
		stringBuilder.Append(serializer.Serialize(obj));

		await _fileIo.WriteAsync(_filepath, stringBuilder, ct);
	}
}
