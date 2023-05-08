using System;
// mandatory
[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationFileNameAttribute : Attribute
{
	public string Value { get; private set; }

	public ConfigurationFileNameAttribute(string value)
	{
		Value = value;
	}
}
