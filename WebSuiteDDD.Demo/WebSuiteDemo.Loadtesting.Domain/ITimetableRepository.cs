using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSuiteDemo.Loadtesting.Domain
{
	public interface ITimetableRepository
	{
		IList<Loadtest> GetLoadtestsForTimePeriod(DateTime searchStartDateUtc, DateTime searchEndDateUtc);
		void AddOrUpdateLoadtests(AddOrUpdateLoadtestsValidationResult addOrUpdateLoadtestsValidationResult);
		void DeleteById(Guid guid);
	}
}
