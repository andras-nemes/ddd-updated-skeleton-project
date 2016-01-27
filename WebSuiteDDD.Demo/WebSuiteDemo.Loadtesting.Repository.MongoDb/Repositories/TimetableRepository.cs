using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDDD.Infrastructure.Common.ApplicationSettings;
using WebSuiteDemo.Loadtesting.Domain;
using WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebSuiteDemo.Loadtesting.Repository.MongoDb.Repositories
{
	public class TimetableRepository : MongoDbRepository, ITimetableRepository
	{
		public TimetableRepository(IConnectionStringRepository connectionStringRepository)
			: base(connectionStringRepository)
		{ }

		public IList<Loadtest> GetLoadtestsForTimePeriod(DateTime searchStartDateUtc, DateTime searchEndDateUtc)
		{
			LoadTestingContext context = LoadTestingContext.Create(base.ConnectionStringRepository);
			
			var dateQueryBuilder = Builders<LoadtestMongoDb>.Filter;
			var startDateBeforeSearchStartFilter = dateQueryBuilder.Lte<DateTime>(l => l.Parameters.StartDateUtc, searchStartDateUtc);
			var endDateAfterSearchStartFilter = dateQueryBuilder.Gte<DateTime>(l => l.Parameters.ExpectedEndDateUtc, searchStartDateUtc);
			var firstPartialDateQuery = dateQueryBuilder.And(new List<FilterDefinition<LoadtestMongoDb>>(){startDateBeforeSearchStartFilter, endDateAfterSearchStartFilter});

			var startDateBeforeSearchEndFilter = dateQueryBuilder.Lte<DateTime>(l => l.Parameters.StartDateUtc, searchEndDateUtc);
			var endDateAfterSearchEndFilter = dateQueryBuilder.Gte<DateTime>(l => l.Parameters.ExpectedEndDateUtc, searchEndDateUtc);
			var secondPartialDateQuery = dateQueryBuilder.And(new List<FilterDefinition<LoadtestMongoDb>>() { startDateBeforeSearchEndFilter, endDateAfterSearchEndFilter });

			var thirdPartialDateQuery = dateQueryBuilder.And(new List<FilterDefinition<LoadtestMongoDb>>() { startDateBeforeSearchStartFilter, endDateAfterSearchEndFilter });

			var startDateAfterSearchStartFilter = dateQueryBuilder.Gte<DateTime>(l => l.Parameters.StartDateUtc, searchStartDateUtc);
			var endDateBeforeSearchEndFilter = dateQueryBuilder.Lte<DateTime>(l => l.Parameters.ExpectedEndDateUtc, searchEndDateUtc);
			var fourthPartialQuery = dateQueryBuilder.And(new List<FilterDefinition<LoadtestMongoDb>>() { startDateAfterSearchStartFilter, endDateBeforeSearchEndFilter });

			var ultimateQuery = dateQueryBuilder.Or(new List<FilterDefinition<LoadtestMongoDb>>() { firstPartialDateQuery, secondPartialDateQuery, thirdPartialDateQuery, fourthPartialQuery });

			List<LoadtestMongoDb> mongoDbLoadtestsInSearchPeriod = context.Loadtests.Find(ultimateQuery)
				.ToList();
			List<Loadtest> loadtestsInSearchPeriod = mongoDbLoadtestsInSearchPeriod.ConvertToDomains().ToList();
			return loadtestsInSearchPeriod;
		}

		public void AddOrUpdateLoadtests(AddOrUpdateLoadtestsValidationResult addOrUpdateLoadtestsValidationResult)
		{
			LoadTestingContext context = LoadTestingContext.Create(base.ConnectionStringRepository);
			if (addOrUpdateLoadtestsValidationResult.ValidationComplete)
			{
				if (addOrUpdateLoadtestsValidationResult.ToBeInserted.Any())
				{
					IEnumerable<LoadtestMongoDb> toBeInserted = addOrUpdateLoadtestsValidationResult.ToBeInserted.PrepareAllForInsertion();
					context.Loadtests.InsertMany(toBeInserted);
				}

				if (addOrUpdateLoadtestsValidationResult.ToBeUpdated.Any())
				{
					foreach (Loadtest toBeUpdated in addOrUpdateLoadtestsValidationResult.ToBeUpdated)
					{
						Guid existingLoadtestId = toBeUpdated.Id;
						var loadtestInDbQuery = context.Loadtests.Find<LoadtestMongoDb>(lt => lt.DomainId == existingLoadtestId);
						LoadtestMongoDb loadtestInDb = loadtestInDbQuery.SingleOrDefault();
						loadtestInDb.AgentId = toBeUpdated.AgentId;
						loadtestInDb.CustomerId = toBeUpdated.CustomerId;
						loadtestInDb.EngineerId = toBeUpdated.EngineerId;
						loadtestInDb.LoadtestTypeId = toBeUpdated.LoadtestTypeId;
						LoadtestParameters ltDomainParameters = toBeUpdated.Parameters;
						loadtestInDb.Parameters.DurationSec = ltDomainParameters.DurationSec;
						loadtestInDb.Parameters.ExpectedEndDateUtc = ltDomainParameters.StartDateUtc.AddSeconds(ltDomainParameters.DurationSec);
						loadtestInDb.Parameters.StartDateUtc = ltDomainParameters.StartDateUtc;
						loadtestInDb.Parameters.UserCount = ltDomainParameters.UserCount;
						loadtestInDb.ProjectId = toBeUpdated.ProjectId;
						loadtestInDb.ScenarioId = toBeUpdated.ScenarioId;
						context.Loadtests.FindOneAndReplace<LoadtestMongoDb>(lt => lt.DbObjectId == loadtestInDb.DbObjectId, loadtestInDb);
					}
				}
			}
			else
			{
				throw new InvalidOperationException("Validation is not complete. You have to call the AddOrUpdateLoadtests method of the Timetable class first.");
			}
		}

		public void DeleteById(Guid guid)
		{
			LoadTestingContext context = LoadTestingContext.Create(base.ConnectionStringRepository);
			context.Loadtests.FindOneAndDelete<LoadtestMongoDb>(lt => lt.DomainId == guid);
		}
	}
}
