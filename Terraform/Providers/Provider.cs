using System;
using System.Collections.Generic;

namespace Terraform.Providers
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
					configValues.Add(config.Name, value);
				}
			}
			return configValues;
		}
	}

	internal class NameAttribute : Attribute
	{
		public string Name { get; set; }
		public NameAttribute(string name)
		{
			this.Name = name;
		}
	}
}