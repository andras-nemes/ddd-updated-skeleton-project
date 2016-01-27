using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WebSuiteDDD.Infrastructure.Common.ApplicationSettings;
using WebSuiteDemo.Loadtesting.Domain;
using WebSuiteDemo.Loadtesting.Repository.MongoDb;
using WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects;
using WebSuiteDemo.Loadtesting.Repository.MongoDb.Repositories;

namespace MongoDbDatabaseTester
{
	class Program
	{
		static void Main(string[] args)
		{
			ITimetableRepository repo = new TimetableRepository(new WebConfigConnectionStringRepository());
			IList<Loadtest> loadtestsInPeriod = repo.GetLoadtestsForTimePeriod(DateTime.UtcNow.AddYears(-1), DateTime.UtcNow.AddYears(1));
			List<Loadtest> toBeInserted = new List<Loadtest>();
			List<Loadtest> toBeUpdated = new List<Loadtest>();
			Loadtest ltNewOne = new Loadtest
				(Guid.NewGuid(),
				new LoadtestParameters(DateTime.UtcNow.AddDays(1), 100, 90),
				Guid.Parse("52d4e276-193d-4ff3-a40e-c45381969d24"),
				Guid.Parse("cb5c3463-d1cb-4c1c-b667-f1c6a065edd1"),
				Guid.Parse("c1cf1179-98a7-4c58-bcaa-a0e7c697293f"),
				Guid.Parse("612cf872-3967-41e7-a30d-28e26df66dcc"),
				Guid.Parse("c3d79996-7045-4bce-a6cd-fcf398717ae5"),
				Guid.Parse("4d27ad00-14d8-4c1c-98b9-64556e957daf"));
			Loadtest ltNewTwo = new Loadtest
				(Guid.NewGuid(),
				new LoadtestParameters(DateTime.UtcNow.AddDays(5), 500, 300),
				Guid.Parse("b9b42875-414f-46b9-8dd2-417668e23e83"),
				Guid.Parse("f966ccf4-7802-4796-8767-637611b611de"),
				Guid.Parse("a4ae54f4-e662-4922-a2ce-4df9af9d23c8"),
				Guid.Parse("95202f85-0c8e-426b-b0ea-ed74f4d1ccbc"),
				Guid.Parse("1e4871ba-de8b-4e2c-98f4-3364b9d82558"),
				Guid.Parse("4d27ad00-14d8-4c1c-98b9-64556e957daf"));
			toBeInserted.Add(ltNewOne);
			toBeInserted.Add(ltNewTwo);

			Loadtest ltUpdOne = new Loadtest
			(Guid.Parse("71b29573-8f67-49ab-8ee0-f8dd0bbceefd"),
			new LoadtestParameters(DateTime.UtcNow.AddDays(14), 50, 300),
			Guid.Parse("52d4e276-193d-4ff3-a40e-c45381969d24"),
			Guid.Parse("5b16880e-b0dd-4d66-bff9-f79eba6490ec"),
			Guid.Parse("40ccb6aa-c9a6-466d-be02-c73122d6edca"),
			Guid.Parse("612cf872-3967-41e7-a30d-28e26df66dcc"),
			Guid.Parse("e2caa1f0-2ee9-4e8f-86a0-51de8aba4eca"),
			Guid.Parse("4d27ad00-14d8-4c1c-98b9-64556e957daf"));
			toBeUpdated.Add(ltUpdOne);

			AddOrUpdateLoadtestsValidationResult validationRes = new AddOrUpdateLoadtestsValidationResult(toBeInserted, toBeUpdated, 
				new List<Loadtest>(), "Validation summary");
			repo.AddOrUpdateLoadtests(validationRes);

			//TestSelectWithWhereClause();
			//TestReplacement();
			//Seed();
			/*
			string mongoDbConnectionString = "mongodb://localhost:27017";
			MongoClient mongoClient = new MongoClient(mongoDbConnectionString);
			IMongoDatabase testDatabase = mongoClient.GetDatabase("Cartoons");
			Task t = testDatabase.CreateCollectionAsync("Disney");
			Task.WaitAll(t);
			//Task.Run(() => testDatabase.CreateCollectionAsync("Disney"));*/

		}

		private static void TestSelectAllNoFilter()
		{
			LoadTestingContext context = LoadTestingContext.Create(new WebConfigConnectionStringRepository());
			Task<List<EngineerMongoDb>> dbEngineersTask = context.Engineers.Find(x => true).ToListAsync();
			Task.WaitAll(dbEngineersTask);
			List<EngineerMongoDb> dbEngineers = dbEngineersTask.Result;

			foreach (EngineerMongoDb eng in dbEngineers)
			{
				Debug.WriteLine(eng.Name);
			}
		}

		private static void TestSelectWithWhereClause()
		{
			LoadTestingContext context = LoadTestingContext.Create(new WebConfigConnectionStringRepository());
			Task<List<AgentMongoDb>> germanAgentsTask = context.Agents.Find(a => a.Location.Country == "Germany").ToListAsync();
			Task.WaitAll(germanAgentsTask);
			List<AgentMongoDb> germanAgents = germanAgentsTask.Result;

			foreach (AgentMongoDb agent in germanAgents)
			{
				Debug.WriteLine(agent.Location.City);
			}

			Guid agentDomainGuid = Guid.Parse("7c9eb839-2c1c-4ae9-8140-170917c975e1");
			Task<AgentMongoDb> singleAgentByIdTask = context.Agents.Find(a => a.DomainId == agentDomainGuid).SingleOrDefaultAsync();
			Task.WaitAll(singleAgentByIdTask);
			AgentMongoDb singleAgentById = singleAgentByIdTask.Result;
			Debug.WriteLine(singleAgentById.Location.City);

			Task<List<AgentMongoDb>> germanAgentsWithBuilderTask = context.Agents
				.Find(Builders<AgentMongoDb>.Filter.Eq<string>(a => a.Location.Country, "Germany")).ToListAsync();
			Task.WaitAll(germanAgentsWithBuilderTask);
			List<AgentMongoDb> germanAgentsWithBuilder = germanAgentsWithBuilderTask.Result;

			foreach (AgentMongoDb agent in germanAgentsWithBuilder)
			{
				Debug.WriteLine(agent.Location.City);
			}
		}

		private static void TestInsertion()
		{
			LoadTestingContext context = LoadTestingContext.Create(new WebConfigConnectionStringRepository());
			CustomerMongoDb newCustomer = new CustomerMongoDb()
			{
				DomainId = Guid.NewGuid(),
				DbObjectId = ObjectId.GenerateNewId(),
				Name = "Brand new customer"
			};
			Task insertionTask = context.Customers.InsertOneAsync(newCustomer);
			Task.WaitAll(insertionTask);
		}

		private static void TestReplacement()
		{
			LoadTestingContext context = LoadTestingContext.Create(new WebConfigConnectionStringRepository());

			Guid existingProjectId = Guid.Parse("1d9fd06f-66d6-41f6-847d-f80dc48d4ede");
			Task<ProjectMongoDb> projectTask = context.Projects.Find(p => p.DomainId == existingProjectId).SingleOrDefaultAsync();
			Task.WaitAll(projectTask);
			ProjectMongoDb project = projectTask.Result;
			project.Description.LongDescription = "This is an updated long description";
			project.Description.ShortDescription = "This is an updated short description";
			Task<ProjectMongoDb> replacementTask = context.Projects.FindOneAndReplaceAsync(p => p.DbObjectId == project.DbObjectId, project);
		}

		private static void TestUpdate()
		{
			LoadTestingContext context = LoadTestingContext.Create(new WebConfigConnectionStringRepository());
			Guid existingAgentGuid = Guid.Parse("7c9eb839-2c1c-4ae9-8140-170917c975e1");
			UpdateDefinition<AgentMongoDb> agentUpdateDefinition = Builders<AgentMongoDb>.Update.Set<string>(a => a.Location.City, "Munich");
			Task<AgentMongoDb> replacementTask = context.Agents.FindOneAndUpdateAsync(a => a.DomainId == existingAgentGuid, agentUpdateDefinition);
			Task.WaitAll(replacementTask);

			AgentMongoDb replacementResult = replacementTask.Result;
			Debug.WriteLine(string.Concat(replacementResult.Location.City, ", ", replacementResult.DomainId));
		}

		private static void Seed()
		{
			LoadTestingContext context = LoadTestingContext.Create(new WebConfigConnectionStringRepository());
			List<AgentMongoDb> agents = new List<AgentMongoDb>();
			AgentMongoDb amazon = new AgentMongoDb();
			amazon.DomainId = Guid.NewGuid();
			amazon.DbObjectId = ObjectId.GenerateNewId();
			amazon.Location = new LocationMongoDb()
			{
				City = "Seattle",
				Country = "USA",
				DbObjectId = ObjectId.GenerateNewId()
			};
			AgentMongoDb rackspace = new AgentMongoDb();
			rackspace.DomainId = Guid.NewGuid();
			rackspace.DbObjectId = ObjectId.GenerateNewId();
			rackspace.Location = new LocationMongoDb()
			{
				City = "Frankfurt",
				Country = "Germany",
				DbObjectId = ObjectId.GenerateNewId()
			};
			AgentMongoDb azure = new AgentMongoDb();
			azure.DomainId = Guid.NewGuid();
			azure.DbObjectId = ObjectId.GenerateNewId();
			azure.Location = new LocationMongoDb()
			{
				City = "Tokyo",
				Country = "Japan",
				DbObjectId = ObjectId.GenerateNewId()
			};
			agents.Add(amazon);
			agents.Add(rackspace);
			agents.Add(azure);
			Task addManyAgentsTask = context.Agents.InsertManyAsync(agents);

			List<CustomerMongoDb> customers = new List<CustomerMongoDb>();
			CustomerMongoDb niceCustomer = new CustomerMongoDb();
			niceCustomer.DomainId = Guid.NewGuid();
			niceCustomer.DbObjectId = ObjectId.GenerateNewId();
			niceCustomer.Name = "Nice customer";

			CustomerMongoDb greatCustomer = new CustomerMongoDb();
			greatCustomer.DomainId = Guid.NewGuid();
			greatCustomer.DbObjectId = ObjectId.GenerateNewId();
			greatCustomer.Name = "Great customer";

			CustomerMongoDb okCustomer = new CustomerMongoDb();
			okCustomer.DomainId = Guid.NewGuid();
			okCustomer.DbObjectId = ObjectId.GenerateNewId();
			okCustomer.Name = "OK Customer";

			customers.Add(niceCustomer);
			customers.Add(greatCustomer);
			customers.Add(okCustomer);
			Task addManyCustomersTask = context.Customers.InsertManyAsync(customers);

			List<EngineerMongoDb> engineers = new List<EngineerMongoDb>();
			EngineerMongoDb john = new EngineerMongoDb();
			john.DomainId = Guid.NewGuid();
			john.Name = "John";
			john.DbObjectId = ObjectId.GenerateNewId();

			EngineerMongoDb mary = new EngineerMongoDb();
			mary.DomainId = Guid.NewGuid();
			mary.Name = "Mary";
			mary.DbObjectId = ObjectId.GenerateNewId();

			EngineerMongoDb fred = new EngineerMongoDb();
			fred.DomainId = Guid.NewGuid();
			fred.Name = "Fred";
			fred.DbObjectId = ObjectId.GenerateNewId();

			engineers.Add(john);
			engineers.Add(mary);
			engineers.Add(fred);
			Task addManyEngineersTask = context.Engineers.InsertManyAsync(engineers);

			List<LoadtestTypeMongoDb> testTypes = new List<LoadtestTypeMongoDb>();
			LoadtestTypeMongoDb stressTest = new LoadtestTypeMongoDb();
			stressTest.DomainId = Guid.NewGuid();
			stressTest.DbObjectId = ObjectId.GenerateNewId();
			stressTest.Description = new DescriptionMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				ShortDescription = "Stress test",
				LongDescription = "To determine or validate an application’s behavior when it is pushed beyond normal or peak load conditions."
			};

			LoadtestTypeMongoDb capacityTest = new LoadtestTypeMongoDb();
			capacityTest.DomainId = Guid.NewGuid();
			capacityTest.DbObjectId = ObjectId.GenerateNewId();
			capacityTest.Description = new DescriptionMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				ShortDescription = "Capacity test",
				LongDescription = "To determine how many users and/or transactions a given system will support and still meet performance goals."
			};

			testTypes.Add(stressTest);
			testTypes.Add(capacityTest);
			Task addManyLoadtestTypesTask = context.LoadtestTypes.InsertManyAsync(testTypes);

			List<ProjectMongoDb> projects = new List<ProjectMongoDb>();
			ProjectMongoDb firstProject = new ProjectMongoDb();
			firstProject.DomainId = Guid.NewGuid();
			firstProject.DbObjectId = ObjectId.GenerateNewId();
			firstProject.Description = new DescriptionMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				ShortDescription = "First project",
				LongDescription = "Long description of first project"
			};

			ProjectMongoDb secondProject = new ProjectMongoDb();
			secondProject.DomainId = Guid.NewGuid();
			secondProject.DbObjectId = ObjectId.GenerateNewId();
			secondProject.Description = new DescriptionMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				ShortDescription = "Second project",
				LongDescription = "Long description of second project"
			};

			ProjectMongoDb thirdProject = new ProjectMongoDb();
			thirdProject.DomainId = Guid.NewGuid();
			thirdProject.DbObjectId = ObjectId.GenerateNewId();
			thirdProject.Description = new DescriptionMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				ShortDescription = "Third project",
				LongDescription = "Long description of third project"
			};
			projects.Add(firstProject);
			projects.Add(secondProject);
			projects.Add(thirdProject);
			Task addManyProjectsTask = context.Projects.InsertManyAsync(projects);

			List<ScenarioMongoDb> scenarios = new List<ScenarioMongoDb>();
			ScenarioMongoDb scenarioOne = new ScenarioMongoDb();
			scenarioOne.DomainId = Guid.NewGuid();
			scenarioOne.DbObjectId = ObjectId.GenerateNewId();
			scenarioOne.UriOne = "www.bbc.co.uk";
			scenarioOne.UriTwo = "www.cnn.com";

			ScenarioMongoDb scenarioTwo = new ScenarioMongoDb();
			scenarioTwo.DomainId = Guid.NewGuid();
			scenarioTwo.DbObjectId = ObjectId.GenerateNewId();
			scenarioTwo.UriOne = "www.amazon.com";
			scenarioTwo.UriTwo = "www.microsoft.com";

			ScenarioMongoDb scenarioThree = new ScenarioMongoDb();
			scenarioThree.DomainId = Guid.NewGuid();
			scenarioThree.DbObjectId = ObjectId.GenerateNewId();
			scenarioThree.UriOne = "www.greatsite.com";
			scenarioThree.UriTwo = "www.nosuchsite.com";
			scenarioThree.UriThree = "www.neverheardofsite.com";

			scenarios.Add(scenarioOne);
			scenarios.Add(scenarioTwo);
			scenarios.Add(scenarioThree);
			Task addManyScenariosTask = context.Scenarios.InsertManyAsync(scenarios);

			List<LoadtestMongoDb> loadtests = new List<LoadtestMongoDb>();
			LoadtestMongoDb ltOne = new LoadtestMongoDb();
			ltOne.DomainId = Guid.NewGuid();
			ltOne.DbObjectId = ObjectId.GenerateNewId();
			ltOne.AgentId = amazon.DomainId;
			ltOne.CustomerId = niceCustomer.DomainId;
			ltOne.EngineerId = john.DomainId;
			ltOne.LoadtestTypeId = stressTest.DomainId;
			DateTime ltOneStart = DateTime.UtcNow;
			ltOne.Parameters = new LoadtestParametersMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				DurationSec = 60,
				StartDateUtc = ltOneStart,
				UserCount = 10,
				ExpectedEndDateUtc = ltOneStart.AddSeconds(60)
			};
			ltOne.ProjectId = firstProject.DomainId;
			ltOne.ScenarioId = scenarioOne.DomainId;

			LoadtestMongoDb ltTwo = new LoadtestMongoDb();
			ltTwo.DomainId = Guid.NewGuid();
			ltTwo.DbObjectId = ObjectId.GenerateNewId();
			ltTwo.AgentId = azure.DomainId;
			ltTwo.CustomerId = greatCustomer.DomainId;
			ltTwo.EngineerId = mary.DomainId;
			ltTwo.LoadtestTypeId = capacityTest.DomainId;
			DateTime ltTwoStart = DateTime.UtcNow.AddMinutes(20);
			ltTwo.Parameters = new LoadtestParametersMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				DurationSec = 120,
				StartDateUtc = ltTwoStart,
				UserCount = 40,
				ExpectedEndDateUtc = ltTwoStart.AddSeconds(120)
			};
			ltTwo.ProjectId = secondProject.DomainId;
			ltTwo.ScenarioId = scenarioThree.DomainId;

			LoadtestMongoDb ltThree = new LoadtestMongoDb();
			ltThree.DomainId = Guid.NewGuid();
			ltThree.DbObjectId = ObjectId.GenerateNewId();
			ltThree.AgentId = rackspace.DomainId;
			ltThree.CustomerId = okCustomer.DomainId;
			ltThree.EngineerId = fred.DomainId;
			ltThree.LoadtestTypeId = stressTest.DomainId;
			DateTime ltThreeStart = DateTime.UtcNow.AddMinutes(30);
			ltThree.Parameters = new LoadtestParametersMongoDb()
			{
				DbObjectId = ObjectId.GenerateNewId(),
				DurationSec = 180,
				StartDateUtc = ltThreeStart,
				UserCount = 50,
				ExpectedEndDateUtc = ltThreeStart.AddSeconds(180)
			};
			ltThree.ProjectId = thirdProject.DomainId;
			ltThree.ScenarioId = scenarioTwo.DomainId;

			loadtests.Add(ltOne);
			loadtests.Add(ltTwo);
			loadtests.Add(ltThree);
			Task addManyLoadtestsTask = context.Loadtests.InsertManyAsync(loadtests);

			Task.WaitAll(addManyAgentsTask, addManyCustomersTask, addManyEngineersTask, addManyLoadtestTypesTask
				, addManyProjectsTask, addManyScenariosTask, addManyLoadtestsTask);
		}
	}
}
