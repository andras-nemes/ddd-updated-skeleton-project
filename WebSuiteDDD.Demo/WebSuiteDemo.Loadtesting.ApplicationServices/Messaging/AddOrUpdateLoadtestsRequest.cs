using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Messaging
{
	public class AddOrUpdateLoadtestsRequest : ServiceRequestBase
	{
		private IEnumerable<LoadtestViewModel> _loadtests;

		public AddOrUpdateLoadtestsRequest(IEnumerable<LoadtestViewModel> loadtests)
		{
			if (loadtests == null) throw new ArgumentNullException("Loadtests");
			_loadtests = loadtests;
		}

		public IEnumerable<LoadtestViewModel> Loadtests
		{
			get
			{
				return _loadtests;
			}
		}
	}
}
