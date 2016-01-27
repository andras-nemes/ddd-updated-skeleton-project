using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.Infrastructure.Common.ApplicationSettings
{
	public class WebConfigConnectionStringRepository : IConnectionStringRepository
	{
		public string ReadConnectionString(string connectionStringName)
		{
			return System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
		}
	}
}
