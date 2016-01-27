using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using WebSuiteDemo.Loadtesting.Domain;
using WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects;

namespace WebSuiteDemo.Loadtesting.Repository.MongoDb.Repositories
{
	public static class ModelConversions
	{
		public static Loadtest ConvertToDomain(this LoadtestMongoDb loadtestDbModel)
		{
			LoadtestParametersMongoDb ltParamsDb = loadtestDbModel.Parameters;
			LoadtestParameters ltParamsDomain = new LoadtestParameters(ltParamsDb.StartDateUtc, ltParamsDb.UserCount, ltParamsDb.DurationSec);
			return new Loadtest(
				loadtestDbModel.DomainId,
				ltParamsDomain,
				loadtestDbModel.AgentId,
				loadtestDbModel.CustomerId,
				loadtestDbModel.EngineerId,
				loadtestDbModel.LoadtestTypeId,
				loadtestDbModel.ProjectId,
				loadtestDbModel.ScenarioId);
		}

		public static IEnumerable<Loadtest> ConvertToDomains(this IEnumerable<LoadtestMongoDb> loadtestDbModels)
		{
			foreach (LoadtestMongoDb db in loadtestDbModels)
			{
				yield return db.ConvertToDomain();
			}
		}

		public static LoadtestMongoDb PrepareForInsertion(this Loadtest domain)
		{
			LoadtestMongoDb ltDb = new LoadtestMongoDb();
			ltDb.DomainId = domain.Id;
			ltDb.DbObjectId = ObjectId.GenerateNewId();
			ltDb.AgentId = domain.AgentId;
			ltDb.CustomerId = domain.CustomerId;
			ltDb.EngineerId = domain.EngineerId;
			ltDb.LoadtestTypeId = domain.LoadtestTypeId;

			int duration = domain.Parameters.DurationSec;
			DateTime start = domain.Parameters.StartDateUtc;
			ltDb.Parameters = new LoadtestParametersMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				DurationSec = duration,
				ExpectedEndDateUtc = start.AddSeconds(duration),
				StartDateUtc = start,
				UserCount = domain.Parameters.UserCount
			};
			ltDb.ProjectId = domain.ProjectId;
			ltDb.ScenarioId = domain.ScenarioId;
			return ltDb;
		}

		public static IEnumerable<LoadtestMongoDb> PrepareAllForInsertion(this IEnumerable<Loadtest> domains)
		{
			foreach (Loadtest domain in domains)
			{
				yield return domain.PrepareForInsertion();
			}
		}

	}
}
