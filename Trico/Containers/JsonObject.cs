using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Trico.Containers;

internal sealed class JsonObject
{
	private readonly JObject _jObj;

	internal JsonObject()
		: this(null)
	{
	}

	internal JsonObject(object? obj)
		: this(JObject.FromObject(obj ?? new object()))
	{
	}

	private JsonObject(JObject? jObj)
	{
		_jObj = jObj ?? new JObject();
	}

	public T? ToObject<T>()
	{
		return _jObj.ToObject<T>();
	}

	public bool TryGet(string key, out string? value)
	{
		value = default;
		if (string.IsNullOrWhiteSpace(key))
		{
			return false;
		}

		var token = _jObj.SelectToken(key);
		if (token is null)
		{
			return false;
		}

		value = token.ToString();
		return true;
	}

	public void Set(string key, string value)
	{
		if (string.IsNullOrWhiteSpace(key))
		{
			throw new ArgumentNullException(nameof(key));
		}

		if (string.IsNullOrWhiteSpace(value))
		{
			throw new ArgumentNullException(nameof(value));
		}

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
			if (jNode is JValue)
			{
				throw new JsonException("given key already contains plain value");
			}

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
}
