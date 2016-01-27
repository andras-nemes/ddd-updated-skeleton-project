using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using WebSuiteDDD.Infrastructure.Common.ApplicationSettings;
using WebSuiteDemo.Loadtesting.Repository.MongoDb.DatabaseObjects;

namespace WebSuiteDemo.Loadtesting.Repository.MongoDb
{
	public class LoadTestingContext
	{
		private IMongoClient Client { get; set; }
		private IMongoDatabase Database { get; set; }
		private const string _databaseName = "Loadtests";
		private static LoadTestingContext _loadTestingContext;

		private LoadTestingContext() { }

		public static LoadTestingContext Create(IConnectionStringRepository connectionStringRepository)
		{
			if (_loadTestingContext == null)
			{
				_loadTestingContext = new LoadTestingContext();
				string connectionString = connectionStringRepository.ReadConnectionString("MongoDbWebSuiteContext");
				_loadTestingContext.Client = new MongoClient(connectionString);
				_loadTestingContext.Database = _loadTestingContext.Client.GetDatabase(_databaseName);
			}			
			return _loadTestingContext;
		}

		public IMongoCollection<AgentMongoDb> Agents
		{
			get { return Database.GetCollection<AgentMongoDb>("Agents"); }
		}

		public IMongoCollection<CustomerMongoDb> Customers
		{
			get { return Database.GetCollection<CustomerMongoDb>("Customers"); }
		}

		public IMongoCollection<EngineerMongoDb> Engineers
		{
			get { return Database.GetCollection<EngineerMongoDb>("Engineers"); }
		}

		public IMongoCollection<LoadtestMongoDb> Loadtests
		{
			get { return Database.GetCollection<LoadtestMongoDb>("Loadtests"); }
		}

		public IMongoCollection<LoadtestTypeMongoDb> LoadtestTypes
		{
			get { return Database.GetCollection<LoadtestTypeMongoDb>("LoadtestTypes"); }
		}

		public IMongoCollection<ProjectMongoDb> Projects
		{
			get { return Database.GetCollection<ProjectMongoDb>("Projects"); }
		}

		public IMongoCollection<ScenarioMongoDb> Scenarios
		{
			get { return Database.GetCollection<ScenarioMongoDb>("Scenarios"); }
		}
	}
}
