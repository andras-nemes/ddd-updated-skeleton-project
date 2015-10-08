using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.ApplicationServices.Messaging;

namespace WebSuiteDemo.Loadtesting.ApplicationServices.Abstractions
{
	public interface ITimetableService
	{
		Task<AddOrUpdateLoadtestsResponse> AddOrUpdateLoadtestsAsync(AddOrUpdateLoadtestsRequest addOrUpdateLoadtestsRequest);
		Task<DeleteLoadtestResponse> DeleteLoadtestAsync(DeleteLoadtestRequest deleteLoadtestRequest);
		Task<GetLoadtestsForTimePeriodResponse> GetLoadtestsForTimePeriodAsync(GetLoadtestsForTimePeriodRequest getLoadtestsForTimePeriodRequest);
	}
}
