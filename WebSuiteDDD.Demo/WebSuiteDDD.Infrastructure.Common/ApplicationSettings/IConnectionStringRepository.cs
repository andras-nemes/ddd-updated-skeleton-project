using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDDD.Infrastructure.Common.ApplicationSettings
{
	public interface IConnectionStringRepository
	{
		string ReadConnectionString(string connectionStringName);
	}
}
