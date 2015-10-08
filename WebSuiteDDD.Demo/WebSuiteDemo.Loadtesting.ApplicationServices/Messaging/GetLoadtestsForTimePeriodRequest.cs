using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Messaging
{
	public class GetLoadtestsForTimePeriodRequest : ServiceRequestBase
	{
		private DateTime _searchStartDateUtc;
		private DateTime _searchEndDateUtc;

		public GetLoadtestsForTimePeriodRequest(DateTime searchStartDateUtc, DateTime searchEndDateUtc)
		{
			_searchStartDateUtc = searchStartDateUtc;
			_searchEndDateUtc = searchEndDateUtc;
		}

		public DateTime SearchStartDateUtc
		{
			get
			{
				return _searchStartDateUtc;
			}
		}

		public DateTime SearchEndDateUtc
		{
			get
			{
				return _searchEndDateUtc;
			}
		}
	}
}
