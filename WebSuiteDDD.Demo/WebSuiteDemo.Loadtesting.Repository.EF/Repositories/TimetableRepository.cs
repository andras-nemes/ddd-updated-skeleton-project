using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.Domain;

namespace WebSuiteDemo.Loadtesting.Repository.EF
{
	public class TimetableRepository : ITimetableRepository
	{
		public IList<Loadtest> GetLoadtestsForTimePeriod(DateTime searchStartDateUtc, DateTime searchEndDateUtc)
		{
			LoadTestingContext context = new LoadTestingContext();			
			IList<Loadtest> loadtestsInSearchPeriod = (from l in context.Loadtests
													   where (l.Parameters.StartDateUtc <= searchStartDateUtc
																	&& SqlFunctions.DateAdd("s", l.Parameters.DurationSec, l.Parameters.StartDateUtc) >= searchStartDateUtc)
																||
																(l.Parameters.StartDateUtc <= searchEndDateUtc
																	&& SqlFunctions.DateAdd("s", l.Parameters.DurationSec, l.Parameters.StartDateUtc) >= searchEndDateUtc)
																||
																(l.Parameters.StartDateUtc <= searchStartDateUtc
																	&& SqlFunctions.DateAdd("s", l.Parameters.DurationSec, l.Parameters.StartDateUtc) >= searchEndDateUtc)
																||
																(l.Parameters.StartDateUtc >= searchStartDateUtc
																	&& SqlFunctions.DateAdd("s", l.Parameters.DurationSec, l.Parameters.StartDateUtc) <= searchEndDateUtc)
																
													   select l).ToList();
			return loadtestsInSearchPeriod;
		}


		public void AddOrUpdateLoadtests(AddOrUpdateLoadtestsValidationResult addOrUpdateLoadtestsValidationResult)
		{
			LoadTestingContext context = new LoadTestingContext();
			if (addOrUpdateLoadtestsValidationResult.ValidationComplete)
			{
				if (addOrUpdateLoadtestsValidationResult.ToBeInserted.Any())
				{
					foreach (Loadtest toBeInserted in addOrUpdateLoadtestsValidationResult.ToBeInserted)
					{
						context.Entry<Loadtest>(toBeInserted).State = System.Data.Entity.EntityState.Added;
					}
				}

				if (addOrUpdateLoadtestsValidationResult.ToBeUpdated.Any())
				{
					foreach (Loadtest toBeUpdated in addOrUpdateLoadtestsValidationResult.ToBeUpdated)
					{
						context.Entry<Loadtest>(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
					}
				}
			}
			else
			{
				throw new InvalidOperationException("Validation is not complete. You have to call the AddOrUpdateLoadtests method of the Timetable class first.");
			}
			context.SaveChanges();
		}


		public void DeleteById(Guid guid)
		{
			LoadTestingContext context = new LoadTestingContext();
			Loadtest loadtest = (from l in context.Loadtests where l.Id == guid select l).FirstOrDefault();
			if (loadtest == null) throw new ArgumentException(string.Format("There's no load test by ID {0}", guid));
			context.Entry<Loadtest>(loadtest).State = System.Data.Entity.EntityState.Deleted;
			context.SaveChanges();
		}
	}
}
