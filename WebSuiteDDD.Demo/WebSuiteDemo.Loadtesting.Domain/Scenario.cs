using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.SharedKernel;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public class Scenario : EntityBase<Guid>
	{
		private Scenario() : base(Guid.NewGuid()) { }

		public string UriOne { get; private set; }
		public string UriTwo { get; private set; }
		public string UriThree { get; private set; }	

		public Scenario(Guid guid, IEnumerable<Uri> loadtestSteps) : base(guid)
		{
			if (loadtestSteps == null || loadtestSteps.Count() == 0)
			{
				throw new ArgumentException("Loadtest scenario must have at least one valid URI.");
			}

			Uri uriOne = loadtestSteps.ElementAt(0);
			if (uriOne == null) throw new ArgumentException("Loadtest scenario must have at least one valid URI.");
			UriOne = uriOne.AbsoluteUri;

			if (loadtestSteps.Count() == 2 && loadtestSteps.ElementAt(1) != null)
			{
				Uri uriTwo = loadtestSteps.ElementAt(1);
				UriTwo = uriTwo.AbsoluteUri;
			}

			if (loadtestSteps.Count() >= 3 && loadtestSteps.ElementAt(1) != null
				&& loadtestSteps.ElementAt(2) != null)
			{
				Uri uriTwo = loadtestSteps.ElementAt(1);
				UriTwo = uriTwo.AbsoluteUri;

				Uri uriThree = loadtestSteps.ElementAt(2);
				UriThree = uriThree.AbsoluteUri;
			}			
		}
	}
}
