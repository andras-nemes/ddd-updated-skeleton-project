using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.Repository.EF;
using WebSuiteDDD.Repository.EF.DataModel;

namespace DemoDatabaseTester
{
	class Program
	{
		static void Main(string[] args)
		{
			LoadtestingContextService domainService = new LoadtestingContextService();
			domainService.TestLoadtestingContext();
		}
	}
}
