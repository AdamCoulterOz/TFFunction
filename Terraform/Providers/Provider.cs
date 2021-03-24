using System;
using System.Collections.Generic;

namespace TF.Providers
{
	public abstract class Provider
	{
		public Dictionary<string, string> GetConfig()
		{
			var configValues = new Dictionary<string, string>();
			foreach (var property in this.GetType().GetProperties())
			{
				var value = property.GetValue(this).ToString();
				if (value != "")
				{
					var config = property.GetCustomAttributes(typeof(NameAttribute), false)[0] as NameAttribute;
					if(config.Lower) value = value.ToLower();
					configValues.Add(config.Name, value);
				}
			}
			return configValues;
		}
	}

	internal class NameAttribute : Attribute
	{
		public string Name { get; set; }
		public bool Lower { get; set; }
		public NameAttribute(string name)
		{
			this.Name = name;
		}
	}
}