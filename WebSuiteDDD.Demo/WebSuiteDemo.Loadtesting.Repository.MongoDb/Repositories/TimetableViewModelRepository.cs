using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSuiteDemo.Loadtesting.Domain;
using WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects;
using MongoDB.Bson;
using MongoDB.Driver;
using WebSuiteDDD.Infrastructure.Common.ApplicationSettings;

namespace WebSuiteDemo.Loadtesting.Repository.MongoDb.Repositories
{
	public class TimetableViewModelRepository : MongoDbRepository, ITimetableViewModelRepository
	{
		public TimetableViewModelRepository(IConnectionStringRepository connectionStringRepository)
			: base(connectionStringRepository)
		{}

		public IList<LoadtestViewModel> ConvertToViewModels(IEnumerable<Loadtest> domains)
		{
			List<LoadtestViewModel> viewModels = new List<LoadtestViewModel>();
			LoadTestingContext context = LoadTestingContext.Create(base.ConnectionStringRepository);
			foreach (Loadtest lt in domains)
			{
				LoadtestViewModel vm = new LoadtestViewModel();
				vm.Id = lt.Id;
				AgentMongoDb agentDb = context.Agents.Find<AgentMongoDb>(a => a.DomainId == lt.AgentId).SingleOrDefault();
				if (agentDb == null) throw new ArgumentException("There is no load test agent with the given ID.");
				vm.AgentCountry = agentDb.Location.Country;
				vm.AgentCity = agentDb.Location.City;

				CustomerMongoDb custDb = context.Customers.Find<CustomerMongoDb>(c => c.DomainId == lt.CustomerId).SingleOrDefault();
				if (custDb == null) throw new ArgumentException("There is no customer with the given ID.");
				vm.CustomerName = custDb.Name;

				if (lt.EngineerId.HasValue)
				{
					EngineerMongoDb engDb = context.Engineers.Find<EngineerMongoDb>(e => e.DomainId == lt.EngineerId.Value).SingleOrDefault();
					if (engDb == null) throw new ArgumentException("There is no engineer with the given ID.");
					vm.EngineerName = engDb.Name;
				}

				LoadtestTypeMongoDb loadtestTypeDb = context.LoadtestTypes.Find(l => l.DomainId == lt.LoadtestTypeId).SingleOrDefault();
				if (loadtestTypeDb == null) throw new ArgumentException("There is no load test type with the given ID.");
				vm.LoadtestTypeShortDescription = loadtestTypeDb.Description.ShortDescription;

				ProjectMongoDb projectDb = context.Projects.Find(p => p.DomainId == lt.ProjectId).SingleOrDefault();
				if (projectDb == null) throw new ArgumentException("There is no project with the given ID.");
				vm.ProjectName = projectDb.Description.ShortDescription;

				ScenarioMongoDb scenarioDb = context.Scenarios.Find(s => s.DomainId == lt.ScenarioId).SingleOrDefault();
				if (scenarioDb == null) throw new ArgumentException("There is no scenario with the given ID.");
				vm.ScenarioUriOne = scenarioDb.UriOne;
				vm.ScenarioUriTwo = scenarioDb.UriTwo;
				vm.ScenarioUriThree = scenarioDb.UriThree;

				vm.UserCount = lt.Parameters.UserCount;
				vm.StartDateUtc = lt.Parameters.StartDateUtc;
				vm.DurationSec = lt.Parameters.DurationSec;

				viewModels.Add(vm);
			}
			return viewModels;
		}

		public IList<Loadtest> ConvertToDomain(IEnumerable<LoadtestViewModel> viewModels)
		{
			List<Loadtest> loadtests = new List<Loadtest>();
			LoadTestingContext context = LoadTestingContext.Create(base.ConnectionStringRepository);
			foreach (LoadtestViewModel vm in viewModels)
			{
				Guid id = vm.Id;
				LoadtestParameters ltParams = new LoadtestParameters(vm.StartDateUtc, vm.UserCount, vm.DurationSec);

				var agentQueryBuilder = Builders<AgentMongoDb>.Filter;
				var agentCityFilter = agentQueryBuilder.Eq<string>(a => a.Location.City.ToLower(), vm.AgentCity.ToLower());
				var agentCountryFilter = agentQueryBuilder.Eq<string>(a => a.Location.Country.ToLower(), vm.AgentCountry.ToLower());
				var agentCompleteFilter = agentQueryBuilder.And(agentCityFilter, agentCountryFilter);
				AgentMongoDb agentDb = context.Agents.Find(agentCompleteFilter).FirstOrDefault();
				if (agentDb == null) throw new ArgumentException("There is no agent with the given properties.");

				var customerQueryBuilder = Builders<CustomerMongoDb>.Filter;
				var customerQuery = customerQueryBuilder.Eq<string>(c => c.Name.ToLower(), vm.CustomerName.ToLower());
				CustomerMongoDb custDb = context.Customers.Find(customerQuery).SingleOrDefault();
				if (custDb == null) throw new ArgumentException("There is no customer with the given properties.");

				Guid? engineerId = null;
				if (!string.IsNullOrEmpty(vm.EngineerName))
				{
					EngineerMongoDb engDb = context.Engineers.Find(e => e.Name.ToLower() == vm.EngineerName.ToLower()).SingleOrDefault();
					if (engDb == null) throw new ArgumentException("There is no engineer with the given properties.");
					engineerId = engDb.DomainId;
				}

				LoadtestTypeMongoDb ltTypeDb = context.LoadtestTypes.Find(t => t.Description.ShortDescription.ToLower() == vm.LoadtestTypeShortDescription.ToLower()).SingleOrDefault();
				if (ltTypeDb == null) throw new ArgumentException("There is no load test type with the given properties.");

				ProjectMongoDb projectDb = context.Projects.Find(p => p.Description.ShortDescription.ToLower() == vm.ProjectName.ToLower()).SingleOrDefault();
				if (projectDb == null) throw new ArgumentException("There is no project with the given properties.");

				var scenarioQueryBuilder = Builders<ScenarioMongoDb>.Filter;
				var scenarioUriOneFilter = scenarioQueryBuilder.Eq<string>(s => s.UriOne.ToLower(), vm.ScenarioUriOne.ToLower());
				var scenarioUriTwoFilter = scenarioQueryBuilder.Eq<string>(s => s.UriTwo.ToLower(), vm.ScenarioUriTwo.ToLower());
				var scenarioUriThreeFilter = scenarioQueryBuilder.Eq<string>(s => s.UriThree.ToLower(), vm.ScenarioUriThree.ToLower());
				var scenarioCompleteFilter = scenarioQueryBuilder.And(scenarioUriOneFilter, scenarioUriTwoFilter, scenarioUriThreeFilter);
				ScenarioMongoDb scenarioDb = context.Scenarios.Find(scenarioCompleteFilter).SingleOrDefault();
				if (scenarioDb == null)
				{					
					scenarioDb = new ScenarioMongoDb()
					{
						DbObjectId = ObjectId.GenerateNewId(),
						DomainId = Guid.NewGuid()
					};
					if (!string.IsNullOrEmpty(vm.ScenarioUriOne))
					{
						scenarioDb.UriOne = vm.ScenarioUriOne;
					}
					if (!string.IsNullOrEmpty(vm.ScenarioUriTwo))
					{
						scenarioDb.UriTwo = vm.ScenarioUriTwo;
					}
					if (!string.IsNullOrEmpty(vm.ScenarioUriThree))
					{
						scenarioDb.UriThree = vm.ScenarioUriThree;
					}
					context.Scenarios.InsertOne(scenarioDb);
				}

				Loadtest converted = new Loadtest(id, ltParams, agentDb.DomainId, custDb.DomainId, engineerId, ltTypeDb.DomainId, projectDb.DomainId, scenarioDb.DomainId);
				loadtests.Add(converted);
			}
			return loadtests;
		}
	}
}
