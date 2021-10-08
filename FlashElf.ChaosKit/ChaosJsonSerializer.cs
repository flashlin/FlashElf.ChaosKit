using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using T1.Standard.Common;
using T1.Standard.DynamicCode;
using T1.Standard.Extensions;
using T1.Standard.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FlashElf.ChaosKit
{
	public class ChaosJsonSerializer : IChaosSerializer
	{
		private readonly ChaosBinarySerializer _binarySerializer;
		private readonly TypeFinder _typeFinder;
		private readonly InsensitiveJsonSerializer _jsonSerializer = new InsensitiveJsonSerializer();

		public ChaosJsonSerializer()
		{
			_typeFinder = new TypeFinder();
			_binarySerializer = new ChaosBinarySerializer();
		}

		public byte[] Serialize<T>(T obj)
		{
			var objType = typeof(T);
			if (typeof(object) == typeof(T))
			{
				var objValue = (object)obj;
				if (objValue == null)
				{
					return new byte[0];
				}
				objType = obj.GetType();
			}

			if (objType == typeof(string))
			{
				return Encoding.UTF8.GetBytes($"{obj}");
			}

			if (IsGenericDictType(objType))
			{
				return SerializeDict(objType, obj);
			}

			var json = _jsonSerializer.Serialize(obj);
            if (string.IsNullOrEmpty(json))
            {
                return Array.Empty<byte>();
            }
			return Encoding.UTF8.GetBytes(json);
		}

		private bool IsGenericDictType(Type t)
		{
			return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Dictionary<,>);
		}

		public object Deserialize(Type type, byte[] data)
		{

			if (IsGenericDictType(type))
			{
				var myDict = (SerializableDictionary)_binarySerializer.Deserialize(typeof(SerializableDictionary), data);
				return DeserializeDictionary(myDict);
			}

			var json = Encoding.UTF8.GetString(data);
			if (typeof(string) == type)
			{
				if (data.Length == 0)
				{
					return String.Empty;
				}
				return json;
			}


			var deserialize =
				DynamicMethod.GetGenericMethod(typeof(InsensitiveJsonSerializer),
					new Type[] {type},
					nameof(InsensitiveJsonSerializer.Deserialize),
					new Type[] { typeof(string) }	
				);
			return deserialize(_jsonSerializer, new object[]{ json });
		}

		private IDictionary DeserializeDictionary(SerializableDictionary serializableDictionary)
		{
			var keyType = _typeFinder.Find(serializableDictionary.KeyTypeFullName);
			var valueType = _typeFinder.Find(serializableDictionary.ValueTypeFullName);

			var dict = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
			var dictInstance = (IDictionary)Activator.CreateInstance(dict);

			foreach (var item in serializableDictionary.Items)
			{
				var key = Deserialize(keyType, item.Key);
				var value = Deserialize(valueType, item.Value);
				dictInstance[key] = value;
			}

			return dictInstance;
		}

		private byte[] SerializeDict(Type dictType, object dictObj)
		{
			var genericArgs = dictType.GetGenericArguments();
			var keyType = genericArgs[0];
			var valueType = genericArgs[1];

			var myDict = new SerializableDictionary()
			{
				KeyTypeFullName = keyType.FullName,
				ValueTypeFullName = valueType.FullName
			};

			var dict = (IDictionary)dictObj;
			foreach (DictionaryEntry item in dict)
			{
				var key = item.Key;
				var value = item.Value;

				var dictItem = new SerializableKeyValuePair()
				{
					Key = Serialize(key),
					Value = Serialize(value)
				};

				myDict.Items.Add(dictItem);
			}

			return _binarySerializer.Serialize(myDict);
		}
	}
}