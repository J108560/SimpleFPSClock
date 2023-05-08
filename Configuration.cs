using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

public abstract class Configuration<C> where C : class, new()
{
	private static C instance;

	public static C Load()
	{
		if (instance == null)
		{
			try
			{
				string configFile = GetConfigFile();
				if (File.Exists(configFile))
				{
					using StreamReader textReader = new StreamReader(configFile);
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(C));
					instance = xmlSerializer.Deserialize(textReader) as C;
				}
				else
				{
					instance = new C();
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				instance = new C();
			}
		}
		return instance;
	}

	public static void Save()
	{
		if (instance == null)
		{
			Debug.LogError("Attempt to save configuration for " + typeof(C).Name + " before an instance was loaded.");
			return;
		}
		try
		{
			string configFile = GetConfigFile();
			using StreamWriter textWriter = new StreamWriter(configFile);
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("", "");
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(C));
			xmlSerializer.Serialize(textWriter, instance, xmlSerializerNamespaces);
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}

	private static string GetConfigFile()
	{
		if (typeof(C).GetCustomAttributes(typeof(ConfigurationFileNameAttribute), inherit: true).FirstOrDefault() is ConfigurationFileNameAttribute configurationFileNameAttribute)
		{
			return configurationFileNameAttribute.Value;
		}
		Debug.LogError("ConfigurationFile attribute missing in " + typeof(C).Name);
		return typeof(C).Name + "Config.xml";
	}
}
