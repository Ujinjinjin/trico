using Comar.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Text;
using YamlDotNet.Core;

namespace Comar.Configuration.Providers;

/// <summary>  </summary>
public sealed class FileConfigurationProvider : IConfigurationProvider
{
	private readonly ISerializerFactory _serializerFactory;
	private JObject? _jObj;
	private string? _filepath;

	public FileConfigurationProvider(ISerializerFactory serializerFactory)
	{
		_serializerFactory = serializerFactory ?? throw new ArgumentNullException(nameof(serializerFactory));
	}

	/// <inheritdoc />
	public void Dispose()
	{
		((IConfigurationProvider)this).Dump();
	}

	/// <inheritdoc />
	public ValueTask DisposeAsync()
	{
		try
		{
			Dispose();
			return default;
		}
		catch (Exception exc)
		{
			return ValueTask.FromException(exc);
		}
	}

	/// <inheritdoc />
	public bool TryGet(string key, out string? value)
	{
		var token = _jObj?.SelectToken(key);
		if (token is null)
		{
			value = default;
			return false;
		}

		value = token.ToString();
		return true;
	}

	/// <inheritdoc />
	public void Set(string key, string? value)
	{
		_jObj ??= new JObject();

		var jToken = SelectToken(_jObj, key.Split('.'), 0);

		switch (jToken.Type)
		{
			case JTokenType.Property:
				if (jToken is JProperty jProperty)
				{
					jProperty.Value = value;
				}
				break;
			case JTokenType.String:
				if (jToken is JValue jValue)
				{
					jValue.Value = value;
				}
				break;
			default:
				throw new InvalidCastException($"node type {jToken.Type} is not supported yet");
		}
	}

	private JToken SelectToken(JToken jNode, IReadOnlyList<string> keyFragments, int index)
	{
		if (index == keyFragments.Count)
		{
			return jNode;
		}

		JToken? jToken = null;
		foreach (var t in jNode.SelectTokens(keyFragments[index]))
		{
			if (jToken is not null)
			{
				throw new JsonException("path returned multiple tokens.");
			}

			jToken = t;
		}

		if (jToken is null)
		{
			if (index + 1 == keyFragments.Count)
			{
				((JObject)jNode).Add(new JProperty(keyFragments[index], string.Empty));
			}
			else
			{
				((JObject)jNode).Add(keyFragments[index], new JObject());
			}

			return SelectToken(jNode, keyFragments, index);
		}

		return SelectToken(jToken, keyFragments, index + 1);
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

		var fileContents = await File.ReadAllTextAsync(filepath, ct);

		object? obj;
		try
		{
			obj = serializer.Deserialize<object>(fileContents);
		}
		catch (JsonSerializationException exception)
		{
			throw new FileLoadException("config file wasn't loaded correctly", exception);
		}
		catch (SemanticErrorException exception)
		{
			throw new FileLoadException("config file wasn't loaded correctly", exception);
		}

		if (obj is null)
		{
			throw new FileLoadException("config file wasn't loaded correctly");
		}

		_jObj = JObject.FromObject(obj);
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

		await using var streamWriter = new StreamWriter(_filepath);
		await streamWriter.WriteAsync(stringBuilder, ct);
	}
}
