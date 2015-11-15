using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.Infrastructure.Common.ApplicationSettings
{
	public class ConfigFileConfigurationRepository : IConfigurationRepository
	{
		public T GetConfigurationValue<T>(string key)
		{
			string value = ConfigurationManager.AppSettings[key];
			if (value == null)
			{
				throw new KeyNotFoundException("Key " + key + " not found.");
			}
			try
			{
				if (typeof(Enum).IsAssignableFrom(typeof(T)))
					return (T)Enum.Parse(typeof(T), value);
				return (T)Convert.ChangeType(value, typeof(T));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public T GetConfigurationValue<T>(string key, T defaultValue)
		{
			string value = ConfigurationManager.AppSettings[key];
			if (value == null)
			{
				return defaultValue;
			}
			try
			{
				if (typeof(Enum).IsAssignableFrom(typeof(T)))
					return (T)Enum.Parse(typeof(T), value);
				return (T)Convert.ChangeType(value, typeof(T));
			}
			catch (Exception ex)
			{
				return defaultValue;
			}
		}
	}
}
